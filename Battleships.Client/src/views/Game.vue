<template>
    <GameMenu
        :isOpen="isGameMenuOpen"
        :options="gameMenuOptions"
        @start-new-game="handleNewGame"
        @resume-game="handleResumeGame"
    />
</template>

<script>
import GameMenu from "../components/GameMenu.vue";

export default {
    name: "Game",
    components: {
        GameMenu,
    },
    data: () => ({
        isGameMenuOpen: true,
        gameHasWinner: false,
        gameMenuOptions: {
            resume: {
                isDisabled: true,
            },
            join: {
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
            this.gameMenuOptions.join.isDisabled = false;
            this.hideGameMenu();
        },
        handleResumeGame() {
            this.hideGameMenu();
        },
    },
};
</script>
