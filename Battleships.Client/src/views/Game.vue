<template>
    <v-main>
        <div>
            <GameMenu
                :isOpen="isGameMenuOpen"
                :options="gameMenuOptions"
                @start-new-game="handleNewGame"
                @resume-game="handleResumeGame"
            />
        </div>
    </v-main>
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
        isGameBoardRedactorOpen: false,
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
    },
};
</script>
