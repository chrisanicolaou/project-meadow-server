package fleetmanager

//
//import (
//	agonesv1 "agones.dev/agones/pkg/apis/agones/v1"
//	"agones.dev/agones/pkg/client/clientset/versioned"
//	agonesRuntime "agones.dev/agones/pkg/util/runtime"
//	"context"
//	"database/sql"
//	"github.com/heroiclabs/nakama-common/runtime"
//	corev1 "k8s.io/api/core/v1"
//	"k8s.io/apimachinery/pkg/api/resource"
//	metav1 "k8s.io/apimachinery/pkg/apis/meta/v1"
//	"k8s.io/client-go/rest"
//)
//
//var _ runtime.FleetManagerInitializer = &AgonesFleetManager{}
//
//type AgonesFleetManager struct {
//	ctx             context.Context
//	aClient         *versioned.Clientset
//	logger          runtime.Logger
//	nk              runtime.NakamaModule
//	db              *sql.DB
//	callbackHandler runtime.FmCallbackHandler
//}
//
//func NewAgonesFleetManager(ctx context.Context, logger runtime.Logger, db *sql.DB, initializer runtime.Initializer, nk runtime.NakamaModule) (runtime.FleetManagerInitializer, error) {
//	config, err := rest.InClusterConfig()
//	if err != nil {
//		logger.Error("Could not create in cluster config: %v", err)
//		return nil, err
//	}
//
//	agonesClient, err := versioned.NewForConfig(config)
//	if err != nil {
//		logger.Error("Could not create in cluster config: %v", err)
//		return nil, err
//	}
//
//	fm := &AgonesFleetManager{
//		ctx:     ctx,
//		aClient: agonesClient,
//		logger:  logger,
//		nk:      nk,
//		db:      db,
//	}
//
//	return fm, nil
//}
//
//func (fm AgonesFleetManager) Init(nk runtime.NakamaModule, callbackHandler runtime.FmCallbackHandler) error {
//	fm.nk = nk
//	fm.callbackHandler = callbackHandler
//
//	return nil
//}
//
//func (fm AgonesFleetManager) Create(ctx context.Context, maxPlayers int, userIds []string, latencies []runtime.FleetUserLatencies, metadata map[string]any, callback runtime.FmCreateCallbackFn) (err error) {
//	gs := &agonesv1.GameServer{ObjectMeta: metav1.ObjectMeta{GenerateName: "simple-game-server", Namespace: "default"},
//		Spec: agonesv1.GameServerSpec{
//			Container: "meadow-game-server",
//			Ports: []agonesv1.GameServerPort{{
//				Name:          "default",
//				PortPolicy:    agonesv1.Dynamic,
//				ContainerPort: 7654,
//				Protocol:      corev1.ProtocolTCP,
//			}},
//			Health: agonesv1.Health{
//				InitialDelaySeconds: 20,
//			},
//			Template: corev1.PodTemplateSpec{
//				Spec: corev1.PodSpec{
//					Containers: []corev1.Container{{Name: "meadow-game-server", Image: "docker.io/chrisanicolaou/meadow-server:alpha", Resources: corev1.ResourceRequirements{
//						Requests: corev1.ResourceList{
//							corev1.ResourceMemory: resource.MustParse("64Mi"),
//							corev1.ResourceCPU:    resource.MustParse("20m"),
//						},
//						Limits: corev1.ResourceList{
//							corev1.ResourceMemory: resource.MustParse("64Mi"),
//							corev1.ResourceCPU:    resource.MustParse("20m"),
//						},
//					}}},
//				},
//			},
//		},
//	}
//
//	newGS, err := fm.aClient.AgonesV1().GameServers("default").Create(ctx, gs, metav1.CreateOptions{})
//
//	placementId := fm.callbackHandler.GenerateCallbackId()
//	fm.logger.WithField("placement_id", placementId).Info("New placement id")
//
//}
//
//func (fm AgonesFleetManager) Get(ctx context.Context, id string) (instance *runtime.InstanceInfo, err error) {
//	//TODO implement me
//	panic("implement me")
//}
//
//func (fm AgonesFleetManager) List(ctx context.Context, query string, limit int, previousCursor string) (list []*runtime.InstanceInfo, nextCursor string, err error) {
//	//TODO implement me
//	panic("implement me")
//}
//
//func (fm AgonesFleetManager) Join(ctx context.Context, id string, userIds []string, metadata map[string]string) (joinInfo *runtime.JoinInfo, err error) {
//	//TODO implement me
//	panic("implement me")
//}
//
//func (fm AgonesFleetManager) Update(ctx context.Context, id string, playerCount int, metadata map[string]any) error {
//	//TODO implement me
//	panic("implement me")
//}
//
//func (fm AgonesFleetManager) Delete(ctx context.Context, id string) error {
//	//TODO implement me
//	panic("implement me")
//}
