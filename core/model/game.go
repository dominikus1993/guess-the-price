package model

type GameID = string
type EventID = string

type GameEvent interface {
	GetGameID() GameID
	GetEventID() EventID
}

type GameStarted struct {
	GameID  GameID
	EventID EventID
}

type Game struct {
	ID GameID
}
