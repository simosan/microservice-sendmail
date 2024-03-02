FROM golang:1.21-alpine

WORKDIR /app

COPY ./go.mod /app
COPY ./go.sum /app
RUN go mod download

COPY ./main.go /app
COPY ./controller /app/controller
COPY ./model /app/model
COPY ./repository /app/repository
COPY ./service /app/service
COPY ./ssl /app/ssl 
COPY ./.env /app

RUN go build -o SUPERVISOR_CHECK /app/main.go
CMD /app/SUPERVISOR_CHECK -e /app/.env