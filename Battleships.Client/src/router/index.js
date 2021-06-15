import Vue from "vue";
import VueRouter from "vue-router";
import Game from "../views/Game.vue";
import Register from "../views/Register.vue";

Vue.use(VueRouter);

const routes = [
    {
        path: "/",
        name: "Game",
        component: Game,
    },
    {
        path: "/register",
        name: "Register",
        component: Register,
    },
];

const router = new VueRouter({
    mode: "history",
    base: process.env.BASE_URL,
    routes,
});

export default router;
