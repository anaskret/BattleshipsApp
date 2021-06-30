<template>
    <v-row justify="space-around">
        <v-col cols="auto">
            <v-dialog
                v-model="open"
                transition="dialog-top-transition"
                max-width="800"
            >
                <template v-slot:activator="{ on }">
                    <v-btn color="#ce721d" v-on="on">Menu</v-btn>
                </template>
                <template v-slot:default="dialog">
                    <v-card class="menu-container">
                        <v-card-actions class="justify-end">
                            <v-btn text @click="closeMenu(dialog)">
                                <font-awesome-icon icon="times" size="2x" />
                            </v-btn>
                        </v-card-actions>
                        <button class="btn" @click="$emit('start-new-game')">
                            Play Game
                        </button>
                        <button class="btn" @click="logout()">Logout</button>
                    </v-card>
                </template>
            </v-dialog>
        </v-col>
    </v-row>
</template>

<script>
export default {
    name: "GameMenu",
    props: {
        isOpen: Boolean,
        options: Object,
    },
    methods: {
        closeMenu(dialog) {
            this.$emit("close-dialog");
            dialog.value = false;
        },
        disableOptions() {
            this.options.join.isDisabled = true;
        },
        logout() {
            localStorage.removeItem("user-token");
            localStorage.removeItem("current-user");
            this.$router.push("/");
        },
    },
    computed: {
        open: {
            get: function () {
                return this.isOpen;
            },
            set: function (newValue) {
                this.$emit("update:isOpen", newValue);
            },
        },
    },
};
</script>

<style scoped>
.btn {
    margin: 2rem;
    font-family: BigSpaceFont;
    font-size: 3rem;
    outline: none;
    color: white;
    text-align: center;
}
.btn:hover,
.btn:focus {
    color: #ce721d;
}
.menu-container {
    display: flex;
    flex-direction: column;
    justify-content: center;
    background-color: #222222;
    box-shadow: inset 0 0 2px 2px rgb(88, 88, 88);
}
.disable {
    pointer-events: none;
    color: #757575;
}
</style>
