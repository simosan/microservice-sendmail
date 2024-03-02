package service

import (
	"SUPERVISOR_CHECK/model"
	"encoding/json"
)

func ToJSON(data []model.UserSupervisors) ([]byte, error) {
	jsonData, err := json.MarshalIndent(data, "", "  ")
	if err != nil {
		return nil, err
	}
	return jsonData, nil
}
