package main

import (
	"SUPERVISOR_CHECK/controller"
	"SUPERVISOR_CHECK/repository"
	"SUPERVISOR_CHECK/service"
	"flag"
	"log"
	"os"

	"github.com/aws/aws-xray-sdk-go/xray"
	"github.com/gin-contrib/cors"
	"github.com/gin-gonic/gin"
	"github.com/joho/godotenv"
)

func main() {
	// 引数確認
	var (
		envopt = flag.String("e", "", "-e .envパス指定")
	)
	flag.Parse()
	opt := make(map[string]string)
	opt["envopt"] = *envopt
	if len(opt) != 1 {
		flag.Usage()
		os.Exit(255)
	}
	// .env読み込み
	err := godotenv.Load(*envopt)
	if err != nil {
		log.Printf(".envの読み込みが出来ませんでした: %v", err)
		flag.Usage()
		os.Exit(255)
	}

	router := gin.Default()
	config := cors.DefaultConfig()
	config.AllowOrigins = []string{os.Getenv("CORS_HOST")}
	config.AllowMethods = []string{"GET"}
	router.Use(cors.New(config))
	router.Use(service.Middleware(xray.NewFixedSegmentNamer("SupervisorCheck")))

	db := repository.DBInit()
	ur := repository.NewUserSupervisor(db)

	router.GET("/usersupervisor", func(c *gin.Context) {
		controller.Get(c, ur)
	})

	router.RunTLS(os.Getenv("GIN_PORT"), os.Getenv("SERVER_CERT"), os.Getenv("SERVER_KEY"))
}
