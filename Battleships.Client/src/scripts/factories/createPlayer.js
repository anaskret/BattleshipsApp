const getValidName = (name, isPlayerTwo) => {
    if (!name && !isPlayerTwo) {
        return "Player 1";
    }

    if (!name && isPlayerTwo) {
        return "Player 2";
    }

    return name;
};

const playerAttack = ({ player, x, y }) => player.receiveAttack({ x, y });

const createPlayer = ({ name = "", board, isPlayerTwo = false } = {}) => {
    if (!board && isPlayerTwo == false)
        throw new Error("Player must have a board");
    if (!board.isReady() && isPlayerTwo == false)
        throw new Error("Player must have a board with ships");

    const playerName = getValidName(name, isPlayerTwo);
    const receiveAttack = board.receiveAttack.bind(board);
    const attack = playerAttack;

    return {
        getName: () => playerName,
        getBoard: () => ({ ...board }),
        attack,
        receiveAttack,
    };
};

export default createPlayer;
