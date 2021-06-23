import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

export default new Vuex.Store({
    state: {
        displayRegister: false,
        token: localStorage.getItem("user-token"),
        currentUser: localStorage.getItem("current-user"),
    },
    mutations: {},
    actions: {},
    modules: {},
});
