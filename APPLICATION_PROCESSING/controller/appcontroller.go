package controller

import (
	"APPLICATION_PROCESSING/repository"
	"APPLICATION_PROCESSING/service"
	"log"
	"net/http"

	"github.com/gin-gonic/gin"
)

func CreateAppl(c *gin.Context, ur repository.ApplProcessRepository) {
	us := service.NewApplProcess(ur)
	if c.PostForm("filename") == "" {
		mus, err := us.CreateAppl(c)
		if err != nil {
			log.Println(err)
			return
		}
		c.IndentedJSON(http.StatusOK, mus)
	} else {
		mus, err := us.CreateApplWithFile(c)
		if err != nil {
			log.Println(err)
			return
		}
		rtn, err := repository.S3Upload(c, mus)
		if err != nil {
			log.Println(err)
			return
		}
		c.IndentedJSON(rtn, mus)
	}
}

func GetStaffAppl(c *gin.Context, ur repository.ApplProcessRepository) {
	us := service.NewApplProcess(ur)
	mus, err := us.GetStaffAppl(c)
	if err != nil {
		log.Println(err)
		return
	}

	c.IndentedJSON(http.StatusOK, mus)
}

func GetSuperiorAppl(c *gin.Context, ur repository.ApplProcessRepository) {
	us := service.NewApplProcess(ur)
	mus, err := us.GetSuperiorAppl(c)
	if err != nil {
		log.Println(err)
		return
	}

	c.IndentedJSON(http.StatusOK, mus)
}

func UpdateApproval(c *gin.Context, ur repository.ApplProcessRepository) {
	us := service.NewApplProcess(ur)
	mus, err := us.UpdateApproval(c)
	if err != nil {
		log.Println(err)
		return
	}

	ctx := c.Request.Context()
	rtn, err := repository.Mailnotification(ctx, mus)
	if rtn != 200 {
		log.Print(err)
		return
	}
	log.Println(mus)

	c.IndentedJSON(http.StatusOK, mus)
}
