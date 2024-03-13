import { useState } from "react";

function Table({ baseUrl, rounds, playerId, dispatch }) {
  const [answer, setAnswer] = useState("");
  const lastRound = rounds[rounds.length - 1];

  function handleSubmit(e) {
    e.preventDefault();

    fetch(
      `http://${baseUrl}:5235/gameround/submitAnswer?roundId=${lastRound.id}&playerId=${playerId}&answer=${answer}`
    )
      .then((response) => {
        return response.json();
      })
      .then((data) => {
        dispatch({
          type: data ? "correctAnswer" : "incorrectAnswer",
          payload: lastRound.id,
        });
      })
      .catch((error) => console.log("Error while submitting the answer"));

    setAnswer("");
  }
  return (
    <div>
      <table>
        <tbody>
          <tr>
            <th>#</th>
            <th>Expression</th>
            <th>Your answer</th>
            <th>Result</th>
          </tr>
          {rounds.map((round, id) => {
            return (
              <tr key={id}>
                <td>{id + 1}</td>
                <td>{round.expression}</td>
                <td>
                  {round === lastRound && round.myAnswer === undefined && (
                    <form onSubmit={handleSubmit}>
                      <input
                        type="number"
                        value={answer}
                        onChange={(e) => setAnswer(e.target.value)}
                      />
                      <input type="submit" hidden></input>
                    </form>
                  )}
                  {(round.myAnswer === undefined || round.myAnswer === null) &&
                    round !== lastRound &&
                    "MISSED"}
                  {round.myAnswer && "CORRECT"}
                  {round.myAnswer !== undefined &&
                    round.myAnswer !== null &&
                    !round.myAnswer &&
                    "INCORRECT"}
                </td>
                <td>
                  {round === lastRound ? "" : round.myAnswer ? "OK" : "FAILED"}
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}

export default Table;
