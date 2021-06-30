<template>
    <v-main>
        <div>
            <GameMenu
                :isOpen.sync="isGameMenuOpen"
                :options="gameMenuOptions"
                @start-new-game="showLobbyCreator"
                @join-game="showLobby"
                @close-dialog="hideGameMenu"
            />
            <TheGame ref="game" />
            <PrepareBoard
                :isOpen.sync="isPrepareBoardOpen"
                :lobbyId="lobbyId"
                @start-game="handleStartGame"
                @close-dialog="closePrepareBoard"
            />
            <CreateLobby
                :isOpen.sync="isLobbyCreatorOpen"
                @start-new-game="handleNewGame"
                @close-dialog="hideLobbyCreator"
            />
        </div>
    </v-main>
</template>

<script>
import GameMenu from "../components/GameMenu.vue";
import PrepareBoard from "../components/PrepareBoard.vue";
import TheGame from "../components/TheGame.vue";
import CreateLobby from "./CreateLobby.vue";

import createPlayer from "../scripts/factories/createPlayer";

export default {
    name: "Game",
    components: {
        GameMenu,
        PrepareBoard,
        TheGame,
        CreateLobby,
    },
    data: () => ({
        isGameMenuOpen: true,
        isPrepareBoardOpen: false,
        isLobbyOpen: false,
        isLobbyCreatorOpen: false,
		isSunk: false,
        gameHasWinner: false,
        gameMenuOptions: {
            join: {
                isDisabled: false,
            },
        },
        lobbyId: null,
    }),
	mounted() {
		console.log(this.$store.state.currentUser)
		this.$gameHub.$on('shoot-player', this.handleRound);
		this.$gameHub.$on('send-status', this.sendStatus);
		this.$gameHub.$on('get-user-win', this.setWinner);
	},
    created() {
        if (localStorage.getItem("user-token") == null) {
            this.hideGameMenu();
            this.$router.push("/");
        }
    },
    methods: {
        hideGameMenu() {
            this.isGameMenuOpen = false;
        },
        showGameMenu() {
            this.isGameMenuOpen = true;
        },
        openPrepareBoard() {
            this.isPrepareBoardOpen = true;
        },
        closePrepareBoard() {
            this.isPrepareBoardOpen = false;
        },
        showLobby() {
            this.hideGameMenu();
            this.isLobbyOpen = true;
        },
        hideLobby() {
            this.isLobbyOpen = false;
        },
        showLobbyCreator() {
            this.hideGameMenu();
            this.isLobbyCreatorOpen = true;
        },
        hideLobbyCreator() {
            this.isLobbyCreatorOpen = false;
        },
        handleNewGame(lobbyId) {
            this.lobbyId = lobbyId;
            this.gameHasWinner = false;
            this.gameMenuOptions.join.isDisabled = false;

            this.hideGameMenu();
            this.hideLobbyCreator();
            this.openPrepareBoard();
            this.$refs.game.resetTheGame();
        },
        handleStartGame(
            playerBoard,
            playerBoardElement,
            playingBoard,
            playingBoardElement
        ) {
            this.closePrepareBoard();
            this.p1 = createPlayer({ board: playerBoard });
            this.p2 = createPlayer({ board: playingBoard, isPlayerTwo: true });

            this.$refs.game.initTheGame(
				this.lobbyId,
                playerBoardElement,
                playingBoardElement,
                this.p1,
                this.p2
            );
        },

        handleRound(lobbyId, x, y, player) {
            debugger;
            if (!this.gameHasAwinner) {
                this.$refs.game.disablePlayingBoard();
				this.$refs.game.updateGameInfo("Opponent Turn!", "rgb(226, 54, 54)");
                this.plHasDamaged = this.makePlTurn(x, y);
                this.$refs.game.updateTheBoardsInfo();

                if (this.p1.getBoard().isAllShipsSunk()) {
                    this.$gameHub.victory(lobbyId);
                }

				if(this.isSunk){
					this.$gameHub.gridStatus(lobbyId, x, y, 2);
                	this.$refs.game.disablePlayingBoard();
					this.isSunk = false;
					return;
				}

                if (this.plHasDamaged) {
					this.$gameHub.gridStatus(lobbyId, x, y, 1);
                	this.$refs.game.disablePlayingBoard();
					return;
				}
				
                this.$refs.game.enablePlayingBoard();
                this.$refs.game.updateGameInfo("Your Turn!", "rgb(43, 197, 87)");
				this.$gameHub.gridStatus(lobbyId, x, y, 0);
                
            }
        },

        makePlTurn(x, y) {
            const attackInfo = this.p1.attack({ player: this.p1, x, y });
            this.$refs.game.updatePlBoard(JSON.stringify({x, y}), attackInfo);
			if(attackInfo.damagedShipData){
				this.isSunk = true;
			}

            return attackInfo === true || attackInfo.damagedShipData;
        },

		sendStatus(lobbyId, x, y, status){
			let cord = JSON.stringify({x, y});
				this.$refs.game.updatePlayingBoard(cord, status);
		},

		setWinner(lobbyId){
			this.gameHasAwinner = true;
			this.$refs.game.updateGameInfo(
				"Congratulations you won The Game",
				"rgb(43, 197, 87)"
			);
			setTimeout(this.showGameMenu, 3000);

			this.$http
				.get("api/playerByName", {
					params: {
                        name: this.$store.state.currentUser,
                    },
				})
				.then((res) => {
                    if (res.status == 200) {
                        this.$http
							.put("api/player", {
								id: res.data.id,
								username: res.data.username,
								wins: ++res.data.wins,
							})
                    }
                })
                .catch((e) => {
                    console.log(e);
                });
			
		},
    },
};
/* 
	statuses
	0 - miss
	1 - hit
	2 - hit and sunk

*/
</script>


