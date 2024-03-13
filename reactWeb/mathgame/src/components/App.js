import { useEffect, useReducer } from "react";
import "../styles.css";
import * as signalR from "@microsoft/signalr";
import Table from "./Table";

const initialState = {
  player: {},
  rounds: [],
  points: 0,
  onlinePlayers: 0,
};

const baseUrl = window.location.hostname;

function reducer(state, action) {
  switch (action.type) {
    case "playerCreated":
      return { ...state, player: action.payload };
    case "playersCountUpdate":
      return { ...state, onlinePlayers: action.payload };
    case "newRound":
      console.log(action.payload);
      return { ...state, rounds: [...state.rounds, action.payload] };
    case "correctAnswer":
      return {
        ...state,
        points: state.points + 1,
        rounds: state.rounds.map((round) => {
          if (round.id === action.payload) {
            const updatedRound = { ...round, myAnswer: true };
            return updatedRound;
          }
          return round;
        }),
      };
    case "incorrectAnswer":
      return {
        ...state,
        rounds: state.rounds.map((round) => {
          if (round.id === action.payload) {
            const updatedRound = { ...round, myAnswer: false };
            return updatedRound;
          }
          return round;
        }),
      };
    case "dataFailed":
      console.log("ERROR");
      return { initialState };
    default:
      throw new Error("Error");
  }
}

function App() {
  const [{ player, rounds, points, onlinePlayers }, dispatch] = useReducer(
    reducer,
    initialState
  );

  useEffect(function () {
    const hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`http://${baseUrl}:5235/gameServerHub`, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
      })
      .build();

    hubConnection.start();

    hubConnection.on("playerCreated", (data) => {
      dispatch({ type: "playerCreated", payload: data });
    });

    hubConnection.on("newRound", (data) => {
      dispatch({ type: "newRound", payload: data });
    });

    hubConnection.on("playersCountUpdate", (data) => {
      dispatch({ type: "playersCountUpdate", payload: data });
    });

    hubConnection.on("firstRound", (data) => {
      dispatch({ type: "newRound", payload: data });
    });
  }, []);

  return (
    <div className="App">
      <span> You have {points} points.</span>
      <span>There are {onlinePlayers} users currently online.</span>
      <Table
        baseUrl={baseUrl}
        rounds={rounds}
        playerId={player.id}
        dispatch={dispatch}
      />
    </div>
  );
}

export default App;
