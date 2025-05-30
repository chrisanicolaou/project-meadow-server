package main

//
import (
	allocationv1 "agones.dev/agones/pkg/apis/allocation/v1"
	"agones.dev/agones/pkg/client/clientset/versioned"
	"agones.dev/agones/pkg/client/informers/externalversions"
	"context"
	"database/sql"
	"github.com/heroiclabs/nakama-common/runtime"
	metav1 "k8s.io/apimachinery/pkg/apis/meta/v1"
	//"k8s.io/apimachinery/pkg/labels"
	"k8s.io/client-go/rest"
	"k8s.io/client-go/tools/cache"
	"time"

	// NOTE: Do not remove. These are required to pin as direct dependencies with Go modules.
	_ "golang.org/x/net/idna"
	_ "golang.org/x/oauth2"
	_ "google.golang.org/genproto/googleapis/rpc/status"
)

func InitModule(ctx context.Context, logger runtime.Logger, db *sql.DB, nk runtime.NakamaModule, initializer runtime.Initializer) error {
	config, err := rest.InClusterConfig()
	if err != nil {
		logger.Error("Could not create in cluster config: %v", err)
	}
	agonesClient, err := versioned.NewForConfig(config)
	if err != nil {
		logger.Error("Could not create the agones api clientset")
	}

	// Create InformerFactory which create the informer
	agonesInformerFactory := externalversions.NewSharedInformerFactory(agonesClient, time.Second*30)

	// Create GameServer informer by informerFactory
	gameServers := agonesInformerFactory.Agones().V1().GameServers()
	gsInformer := gameServers.Informer()

	// Add EventHandler to informer
	// When the object's event happens, the function will be called
	gsInformer.AddEventHandler(cache.ResourceEventHandlerFuncs{
		AddFunc:    func(new interface{}) { logger.Info("GameServer Added") },
		UpdateFunc: func(old, new interface{}) { logger.Info("GameServer Updated") },
		DeleteFunc: func(old interface{}) { logger.Info("GameServer Deleted") },
	})

	// Start Go routines for informer
	agonesInformerFactory.Start(ctx.Done())
	// Wait until finish caching with List API
	agonesInformerFactory.WaitForCacheSync(ctx.Done())

	// Create Lister which can list objects from the in-memory-cache
	//gsLister := gameServers.Lister()

	//for {
	//	// Get List objects of GameServers from GameServer Lister
	//	gs, err := gsLister.List(labels.Everything())
	//	if err != nil {
	//		panic(err)
	//	}
	//	// Show GameServer's name & status & IPs
	//	for _, g := range gs {
	//		logger.Info("------------------------------")
	//		logger.Info("Name: %s", g.GetName())
	//		logger.Info("Status: %s", g.Status.State)
	//		logger.Info("External IP: %s", g.Status.Address)
	//	}
	//	time.Sleep(time.Second * 25)
	//}

	err = initializer.RegisterMatchmakerMatched(func(ctx context.Context, logger runtime.Logger, db *sql.DB, nk runtime.NakamaModule, entries []runtime.MatchmakerEntry) (string, error) {
		allocation := &allocationv1.GameServerAllocation{
			ObjectMeta: metav1.ObjectMeta{
				GenerateName: "allocation-", // optional, adds a prefix
			},
			Spec: allocationv1.GameServerAllocationSpec{
				Selectors: []allocationv1.GameServerSelector{
					{
						LabelSelector: metav1.LabelSelector{
							MatchLabels: map[string]string{
								"agones.dev/fleet": "meadow-game-server",
							},
						},
					},
				},
			},
		}

		allocatedGS, err := agonesClient.AllocationV1().GameServerAllocations("default").Create(ctx, allocation, metav1.CreateOptions{})
		if err != nil {
			logger.Error("Error creating GameServerAllocation: %v", err)
		}

		logger.Info("AllocatedGS: %v", allocatedGS)

		for _, entry := range entries {
			userId := entry.GetPresence().GetUserId()
			subject := "connection-info"
			content := map[string]interface{}{
				"IpAddress": "127.0.0.1", // GS allocation returns pod IP, but we want localhost for now
				"Port":      allocatedGS.Status.Ports[0].Port,
				"SessionId": "1",
			}
			err := nk.NotificationSend(ctx, userId, subject, content, 1, "", false)
			if err != nil {
				logger.Error("Failed to send notification: %v", err)
				return "", nil
			}
		}
		return "", nil
	})
	if err != nil {
		logger.Error("Failed to return connection details: %v", err)
		return err
	}
	return nil
}
