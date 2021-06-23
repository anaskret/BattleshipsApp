<template>
    <div class="game-wrapper">
        <div class="game-container">
            <div class="playerContainer"></div>
            <div class="game-info"></div>
            <div class="opponentContainer"></div>
        </div>
    </div>
</template>

<script>
export default {
    name: "TheGame",

    data: () => ({
        isGameInitiated: false,
        playerElement: null,
        opponentElement: null,
        playerBoardElement: null,
        opponentBoardElement: null,
        playerBoardInfoElement: null,
        opponentBoardInfoElement: null,
        gameInfo: null,
        playerContainer: null,
        opponentContainer: null,
        lobbyId: null,
    }),

    methods: {
        initTheGame(
            lobbyId,
            playerBoardElement,
            opponentBoardElement,
            playerContainer,
            opponentContainer
        ) {
            this.lobbyId = lobbyId;
            this.playerElement = document.querySelector(".playerContainer");
            this.opponentElement = document.querySelector(".opponentContainer");
            this.gameInfo = document.querySelector(".game-info");
            this.playerBoardElement = playerBoardElement;
            this.opponentBoardElement = opponentBoardElement;
            this.playerContainer = playerContainer;
            this.opponentContainer = opponentContainer;

            this.renderTheBoards();
            this.renderTheBoardsInfo();
            this.updateTheBoardsInfo();
            this.updateGameInfo("Your Turn!");
            this.addPlayerShootListener();

            this.isGameInitiated = true;
        },

        renderTheBoards() {
            this.playerElement.appendChild(this.playerBoardElement);
            this.opponentElement.appendChild(this.opponentBoardElement);
        },

        resetTheGame() {
            if (this.isGameInitiated) {
                this.updateGameInfo("");
                this.playerBoardElement.remove();
                this.opponentBoardElement.remove();
                this.playerBoardInfoElement.remove();
                this.opponentBoardInfoElement.remove();
            }
        },

        addPlayerShootListener() {
            this.opponentBoardElement.addEventListener("click", (e) => {
                if (e.target.classList.contains("spot")) {
                    const { x, y } = JSON.parse(e.target.dataset.cord);
					this.$http
						.get("api/lobby", {
							params: {
								id: this.lobbyId,
							},
						})
						.then((res) => {
							if (res.status == 200) {
								if(res.data.playerOne == this.$store.state.currentUser){
                    				this.$gameHub.shoot(this.lobbyId, x, y, res.data.playerTwo);
								}else {
									this.$gameHub.shoot(this.lobbyId, x, y, res.data.playerOne);
								}
							}
						});
				}		
			})
		},updatePlayingBoard(cord, response) {
			debugger;
            if (response == 1 || response == 2) {
                const spot = this.opponentBoardElement.querySelector(
                    `.spot[data-cord=${JSON.stringify(cord)}]`
                );

                spot.append("x");
                spot.style.backgroundColor = "rgb(218, 100, 100)";
                spot.style.lineHeight = "1";
                spot.style.pointerEvents = "none";
                spot.classList.toggle("resize");

                if (response == 2) {
                    this.updateGameInfo("Ship Sunk!", "rgb(95 93 228)");
                }
            }

            if (response == 0) {
                const spot = this.opponentBoardElement.querySelector(
                    `.spot[data-cord=${JSON.stringify(cord)}]`
                );

                spot.append("*");
                spot.style.pointerEvents = "none";
                spot.classList.toggle("resize");
            }
        },
        

        updatePlBoard(cord, response) {
            if (response === true || response.damagedShipData) {
                const part = this.playerBoardElement.querySelector(
                    `.part[data-cord=${JSON.stringify(cord)}]`
                );

                part.append("x");
                part.style.backgroundColor = "rgb(218, 100, 100)";
                part.classList.toggle("resize");

                if (response.damagedShipData) {
                    const { clearedBorders } = response;

                    clearedBorders.forEach((borderCord) => {
                        const spotEl = this.playerBoardElement.querySelector(
                            `.spot[data-cord=${JSON.stringify(
                                JSON.stringify(borderCord)
                            )}]`
                        );

                        if (!spotEl.firstChild) {
                            spotEl.append("*");
                            spotEl.classList.toggle("resize");
                        }
                    });
                }
            }

            if (response === false) {
                const spot = this.playerBoardElement.querySelector(
                    `.spot[data-cord=${JSON.stringify(cord)}]`
                );

                spot.append("*");
                spot.classList.toggle("resize");
            }
        },

        updateTheBoardsInfo() {
            const updateInfoFor = (player, boardInfoElement) => {
                const nameEl = boardInfoElement.firstElementChild;
                const aliveShipsEl = boardInfoElement.lastElementChild;
                const name = player.getName();
                const aliveShips = player.getBoard().getAliveShipsCount();

				if(name == 'Player 1'){
					nameEl.textContent = 'Opponent Board';
				}

                if (`Alive Ships: ${aliveShipsEl.textContent}` !== aliveShips) {
                    aliveShipsEl.textContent = `Your Alive Ships: ${aliveShips}`;
                }
            };

            updateInfoFor(this.playerContainer, this.opponentBoardInfoElement);
        },

        renderTheBoardsInfo() {
            const createBoardInfo = () => {
                const infoElement = document.createElement("div");
                const name = document.createElement("h3");
                const aliveShips = document.createElement("h4");

                infoElement.classList.add("board-info");
                name.classList.add("name");
                aliveShips.classList.add("alive-ships");

                infoElement.appendChild(name);
                infoElement.appendChild(aliveShips);

                return infoElement;
            };

            this.playerBoardInfoElement = createBoardInfo();
            this.opponentBoardInfoElement = createBoardInfo();

            this.playerElement.appendChild(this.playerBoardInfoElement);
            this.opponentElement.appendChild(this.opponentBoardInfoElement);
        },

        updateGameInfo(msg = "", color = "rgb(43, 197, 87)") {
            this.gameInfo.textContent = msg;
            this.gameInfo.style.color = color;

            if (!this.gameInfo.classList.contains("pulse")) {
                this.gameInfo.classList.add("pulse");
                this.gameInfo.classList.add("top-bot-borders");
            }

            if (!msg) {
                this.gameInfo.classList.remove("pulse");
                this.gameInfo.classList.remove("top-bot-borders");
            }
        },

        disablePlayingBoard() {
            this.opponentBoardElement.style.pointerEvents = "none";
        },

        enablePlayingBoard() {
            this.opponentBoardElement.style.pointerEvents = "auto";
        },
    },
};
</script>

<style scoped>
.game-container {
    font-family: bfont;
    --spot-size: 3rem;
    padding: 2rem;
    display: flex;
    justify-content: space-around;
}

.playerContainer,
.opponentContainer {
    display: flex;
    flex-direction: column;
}

.playerContainer >>> .ship,
.opponentContainer >>> .ship {
    cursor: initial;
}

.opponentContainer >>> .spot,
.playerContainer >>> .spot {
    text-shadow: 0 0 2px black;
    text-align: center;
    font-size: var(--spot-size);
}

.opponentContainer >>> .spot {
    cursor: crosshair;
}

.opponentContainer >>> .spot:hover {
    background-color: rgb(98, 0, 255);
}

.opponentContainer >>> .spot .ship {
    display: grid;
    grid-auto-flow: column;
    background-color: rgb(187, 82, 82);
    grid-gap: 4px;
    z-index: 69;
}

.opponentContainer >>> .spot .part {
    width: var(--spot-size);
    height: var(--spot-size);
    background-color: rgb(218, 100, 100);
    border: 4px solid rgb(70, 70, 70);
}

.opponentContainer >>> .spot .part,
.playerContainer >>> .spot .part {
    text-shadow: 0 0 2px black;
    text-align: center;
    font-size: var(--spot-size);
    line-height: 0.745;
}

.opponentContainer >>> .board-info,
.playerContainer >>> .board-info {
    text-shadow: 0 0 2px black;
    text-align: center;
    padding-left: var(--spot-size);
}

.playerContainer >>> .board-info {
    color: rgb(43, 197, 87);
}

.opponentContainer >>> .board-info {
    color: rgb(226, 54, 54);
}

.opponentContainer >>> .board-info .name,
.playerContainer >>> .board-info .name {
    border-bottom: 1px solid;
    font-size: 2.6rem;
}

.opponentContainer >>> .board-info .alive-ships,
.playerContainer >>> .board-info .alive-ships {
    font-size: 2.4rem;
}

.game-info {
    font-size: 3rem;
    align-self: center;
    text-align: center;
    text-shadow: 0 0 6px black;
    width: 100%;
}

.top-bot-borders {
    padding: 1rem;
    margin: 1rem;
    border-top: 1px solid;
    border-bottom: 1px solid;
}

.pulse {
    animation: pulse 500ms alternate infinite;
}

.opponentContainer >>> .resize,
.playerContainer >>> .resize {
    animation: resize 200ms 1 reverse;
}

@keyframes pulse {
    100% {
        box-shadow: inset 0 0 2px 1px rgb(82, 82, 82),
            0 0 2px 1px rgb(255, 255, 255);
        color: rgb(255, 255, 255);
    }
}

@keyframes resize {
    100% {
        transform: scale(1.16);
        background-color: rgb(226, 54, 54);
    }
}

@media screen and (max-width: 1400px) {
    .opponentContainer >>> .board-info .alive-ships,
    .playerContainer >>> .board-info .alive-ships {
        font-size: 1.8rem;
    }

    .game-info {
        font-size: 2.2rem;
        padding: 0.4rem;
    }

    .opponentContainer >>> .board-info .name,
    .playerContainer >>> .board-info .name {
        font-size: 2rem;
    }
}

@media screen and (max-width: 1280px) {
    .game-container {
        padding: 1rem;
        display: grid;
        grid-auto-columns: auto auto;
        grid-auto-rows: auto auto;
    }

    .game-info {
        grid-column: 1 / 3;
        order: -1;
    }
}

@media screen and (max-width: 1000px) {
    .opponentContainer >>> .spot .ship {
        grid-gap: 2px;
    }

    .opponentContainer >>> .spot .part,
    .playerContainer >>> .spot .part {
        line-height: 0.7;
    }

    .game-info {
        order: 0;
    }

    .game-container {
        justify-content: center;
    }
}

@media screen and (max-width: 820px) {
    .game-container {
        padding: 2rem;
        display: flex;
        flex-direction: column;
        align-items: center;
    }
}

@media screen and (max-width: 359px) {
    .opponentContainer >>> .spot .ship {
        grid-gap: 1px;
    }

    .opponentContainer >>> .spot .part {
        border-width: 1px;
    }

    .opponentContainer >>> .spot .part,
    .playerContainer >>> .spot .part {
        line-height: 0.9;
    }
}
</style>
