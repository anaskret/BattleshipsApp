<template>
    <v-row justify="space-around">
        <v-col cols="auto">
            <v-dialog transition="dialog-top-transition" max-width="800">
                <template v-slot:activator="{ on, attrs }">
                    <v-btn color="#ce721d" v-bind="attrs" v-on="on">Menu</v-btn>
                </template>
                <template v-slot:default="dialog">
                    <v-card class="menu-container">
                        <v-card-actions class="justify-end">
                            <v-btn text @click="closeDialog(dialog)">
                                <font-awesome-icon icon="times" size="2x" />
                            </v-btn>
                        </v-card-actions>
                        <button class="btn" @click="$emit('start-new-game')">
                            New Game
                        </button>
                        <button
                            class="btn"
                            :class="{
                                disable: options.resume.isDisabled,
                            }"
                            @click="$emit('resume-game')"
                        >
                            Resume
                        </button>
                        <button
                            class="btn"
                            @click="$emit('join-game')"
                            :class="{
                                disable: options.join.isDisabled,
                            }"
                        >
                            Join Game
                        </button>
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
        closeDialog(dialog) {
            dialog.value = false;
            this.disableOptions();
        },
        disableOptions() {
            this.options.resume.isDisabled = true;
            this.options.join.isDisabled = true;
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
