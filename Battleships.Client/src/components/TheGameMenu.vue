<template>
    <div class="menu-container">
        <button class="btn" @click="$emit('start-new-game')">New Game</button>
        <button
            class="btn"
            :class="{ disable: options.resume.isDisabled }"
            @click="$emit('resume-game')"
        >
            Resume
        </button>
    </div>
</template>

<script>
export default {
    name: "TheGameMenu",
    props: {
        isOpen: Boolean,
        options: Object,
    },
};
</script>

<style scoped>
.btn {
    margin: 2rem;
    font-size: 3rem;
    outline: none;
    text-shadow: 0 2px 2px black;
    text-align: center;
}
.btn:hover,
.btn:focus {
    color: rgb(206, 114, 29);
}
.menu-container {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    background-color: rgba(34, 34, 34, 0.8);
    box-shadow: inset 0 0 2px 2px rgb(56, 56, 56);
}
.disable {
    pointer-events: none;
    color: rgb(117, 117, 117);
}
</style>
