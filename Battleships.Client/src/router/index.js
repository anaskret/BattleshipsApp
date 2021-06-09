import { createRouter, createWebHistory } from "vue-router";
import GameMenu from "../views/GameMenu.vue";

const routes = [
    {
        path: "/",
        name: "GameMenu",
        component: GameMenu,
    },
];

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes,
});

export default router;
