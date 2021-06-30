import Vue from "vue";
import VueRouter from "vue-router";
import Game from "../views/Game.vue";
import Authenticate from "../views/Authenticate.vue";

Vue.use(VueRouter);

const routes = [
    {
        path: "/game",
        name: "Game",
        component: Game,
    },
    {
        path: "/",
        name: "Authenticate",
        component: Authenticate,
    },
];

const router = new VueRouter({
    mode: "history",
    base: process.env.BASE_URL,
    routes,
});

export default router;
