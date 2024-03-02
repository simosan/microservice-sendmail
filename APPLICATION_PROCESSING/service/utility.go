package service

import (
	"APPLICATION_PROCESSING/model"
	"encoding/json"
	"log"
)

func ToJSON(data []model.ApplHistories) ([]byte, error) {
	jsonData, err := json.MarshalIndent(data, "", "  ")
	if err != nil {
		log.Println("Error marshaling JSON:", err)
		return nil, err
	}
	return jsonData, nil
}
