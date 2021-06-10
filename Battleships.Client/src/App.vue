<template>
    <v-app>
        <v-main>
            <router-view />
        </v-main>
    </v-app>
</template>

<script>
export default {
    name: "App",
};
</script>

<style>
@font-face {
    font-family: BigSpaceFont;
    src: url("./assets/fonts/BigSpace.ttf");
}

*,
*::before,
*::after {
    padding: 0;
    margin: 0;
    box-sizing: border-box;
    font-size: inherit;
    /* background: #2d2d2d; */
}
.v-main {
    height: 100%;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
}
html {
    font-size: 12px;
}
@media screen and (max-width: 860px) {
    html {
        font-size: 10px;
    }
}
@media screen and (max-width: 410px) {
    html {
        font-size: 8px;
    }
}
@media screen and (max-width: 359) {
    html {
        font-size: 6px;
    }
}
@media screen and (max-width: 350px) {
    html {
        font-size: 5px;
    }
}
</style>
