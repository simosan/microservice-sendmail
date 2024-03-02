package main

import (
	"APPLICATION_PROCESSING/controller"
	"APPLICATION_PROCESSING/repository"
	"APPLICATION_PROCESSING/service"
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

	db := repository.DBInit()
	ur := repository.NewApplProcess(db)

	router := gin.Default()
	config := cors.DefaultConfig()
	config.AllowOrigins = []string{os.Getenv("CORS_HOST")}
	config.AllowMethods = []string{"GET", "POST", "PUT"}
	router.Use(cors.New(config))
	router.Use(service.Middleware(xray.NewFixedSegmentNamer("ApplicationProcessing")))
	router.OPTIONS("/*any", func(c *gin.Context) {
		c.Status(200)
	})

	routes := router.Group("/v1")
	{
		routes.POST("/appl", func(c *gin.Context) {
			controller.CreateAppl(c, ur)
		})
		routes.GET("/staff", func(c *gin.Context) {
			controller.GetStaffAppl(c, ur)
		})
		routes.GET("/supervisor", func(c *gin.Context) {
			controller.GetSuperiorAppl(c, ur)
		})
		routes.POST("/approval", func(c *gin.Context) {
			controller.UpdateApproval(c, ur)
		})
	}
	router.RunTLS(os.Getenv("GIN_PORT"), os.Getenv("SERVER_CERT"), os.Getenv("SERVER_KEY"))
}
