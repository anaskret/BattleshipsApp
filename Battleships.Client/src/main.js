import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import vuetify from "./plugins/vuetify";
import { library } from "@fortawesome/fontawesome-svg-core";
import { faTimes } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import Vuelidate from "vuelidate";
import axios from "axios";

import lobbyHub from "./plugins/GameHub";

library.add(faTimes);

Vue.component("font-awesome-icon", FontAwesomeIcon);

Vue.config.productionTip = false;

Vue.use(Vuelidate);

Vue.use(lobbyHub);

axios.defaults.baseURL = "http://192.168.0.16:5000";
Vue.prototype.$http = axios;

new Vue({
    router,
    store,
    vuetify,
    Vuelidate,
    render: (h) => h(App),
}).$mount("#app");
