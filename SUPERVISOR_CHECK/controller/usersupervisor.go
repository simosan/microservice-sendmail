package controller

import (
	"SUPERVISOR_CHECK/repository"
	"SUPERVISOR_CHECK/service"
	"log"
	"net/http"

	"github.com/gin-gonic/gin"
)

func Get(c *gin.Context, ur repository.UserSupervisorsRepository) {
	depname := c.Query("q")
	us := service.NewUserSupervisor(ur)
	mus, err := us.GetSupervisors(c, depname)
	if err != nil {
		log.Println(err)
		return
	}
	c.IndentedJSON(http.StatusOK, mus)
}
