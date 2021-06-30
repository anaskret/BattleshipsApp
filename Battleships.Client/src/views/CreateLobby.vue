<template>
    <v-row justify="space-around">
        <v-col cols="auto">
            <v-dialog
                v-model="isOpen"
                transition="dialog-top-transition"
                max-width="800"
            >
                <div class="container">
                    <div class="d-flex justify-content-end">
                        <v-btn @click="$emit('close-dialog')">
                            <font-awesome-icon icon="times" size="2x" />
                        </v-btn>
                    </div>
                    <v-toolbar color="orange" dark>
                        <v-toolbar-title>Enter Lobby</v-toolbar-title>
                        <v-spacer></v-spacer>
                    </v-toolbar>
                    <form id="create-lobby-form" v-on:submit.prevent="submit">
                        <div class="col-12 form-group">
                            <label class="col-form-label col-form-label-lg"
                                >Lobby Name</label
                            >
                            <input
                                type="text"
                                v-model.trim="lobbyName"
                                class="form-control form-control-lg"
                            />
                        </div>
                        <div class="col-12 form-group text-center">
                            <input
                                type="submit"
                                class="btn btn-vue btn-lg col-4"
                                value="Create Lobby"
                            />
                        </div>
                    </form>
                </div>
            </v-dialog>
        </v-col>
    </v-row>
</template>

<script>
export default {
    name: "CreateLobby",
    props: {
        isOpen: Boolean,
    },
    data() {
        return {
            lobbyName: "",
			user1: "",
			user2: "",
        };
    },
    methods: {
        submit() {
            this.$http
                .get("api/lobbyByName", {
                    params: {
                        name: this.lobbyName,
                    },
                })
                .then((res) => {
                    if (res.status == 200) {
						if(res.data.playerOne){
							this.$http
							.put("api/lobby", {
								id: res.data.id,
								name: this.lobbyName,
								playerOne: res.data.playerOne,
								playerTwo: this.$store.state.currentUser
							})
						} else {
						this.$http
							.put("api/lobby", {
								id: res.data.id,
								name: this.lobbyName,
								playerOne: this.$store.state.currentUser,
								playerTwo: res.data.playerTwo
								});
						}

                        this.$gameHub.joinGame(res.data.id);
                        this.$emit("start-new-game", res.data.id);
                    } else if (res.status == 204) {
                        this.createLobby();
                    }
                })
                .catch((e) => {
                    console.log(e);
                });
        },
        createLobby() {
            this.$http
                .post("api/lobby", {
                    name: this.lobbyName,
                    playerOne: this.$store.state.currentUser,
                    playerTwo: "",
                })
                .then((res) => {
                    if (res.status == 200) {
                        this.$gameHub.joinGame(res.data.id);
                        this.$emit("start-new-game", res.data.id);
                    }
                })
                .catch((e) => {
                    console.log(e);
                });
        },
    },
};
</script>

<style scoped>
.container {
    display: flex;
    flex-direction: column;
    justify-content: center;
    background-color: #222222;
    box-shadow: inset 0 0 2px 2px rgb(88, 88, 88);
}
</style>
