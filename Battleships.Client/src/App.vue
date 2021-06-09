<template>
    <v-app>
        <v-content class="content">
            <TheGameMenu
                :isOpen="isGameMenuOpen"
                :options="gameMenuOptions"
                @start-new-game="handleNewGame"
                @resume-game="handleResumeGame"
            />
        </v-content>
    </v-app>
</template>

<script>
import TheGameMenu from "./components/TheGameMenu.vue";
export default {
    name: "App",
    components: {
        TheGameMenu,
    },
    data: () => ({
        isGameMenuOpen: true,
        gameHasWinner: false,
        gameMenuOptions: {
            resume: {
                isDisabled: true,
            },
        },
    }),
    methods: {
        hideGameMenu() {
            this.isGameMenuOpen = false;
        },
        handleShowGameMenu() {
            this.isGameMenuOpen = true;
        },
        handleNewGame() {
            this.gameHasWinner = false;
            this.gameMenuOptions.resume.isDisabled = false;
            this.hideGameMenu();
        },
        handleResumeGame() {
            this.hideGameMenu();
        },
    },
};
</script>

<style>
*,
*::before,
*::after {
    padding: 0;
    margin: 0;
    box-sizing: border-box;
    font-size: inherit;
    background: rgb(46, 48, 43);
}
.content {
    height: 100%;
    display: flex;
    flex-direction: column;
    justify-content: center;
}
html {
    font-size: 10px !important;
}
@media screen and (max-width: 860px) {
    html {
        font-size: 7px !important;
    }
}
@media screen and (max-width: 410px) {
    html {
        font-size: 6px !important;
    }
}
@media screen and (max-width: 359) {
    html {
        font-size: 5px !important;
    }
}
@media screen and (max-width: 350px) {
    html {
        font-size: 4px !important;
    }
}
</style>
