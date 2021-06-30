<template>
    <div id="authenticate" class="h-100">
        <div class="container-fluid h-100">
            <div class="row h-100">
                <div
                    class="
                        col-md-3
                        vue-bg
                        h-100
                        d-flex
                        justify-content-center
                        align-items-center
                    "
                >
                    <img alt="Vue logo" src="../assets/logo.png" width="100" />
                </div>
                <div
                    class="
                        col-md-9
                        h-100
                        d-flex
                        justify-content-center
                        align-items-center
                    "
                >
                    <div
                        class="
                            col-md-8
                            rounded
                            px-5
                            py-4
                            shadow
                            bg-dark
                            text-left
                        "
                    >
                        <template v-if="displayRegister">
                            <RegisterForm />
                        </template>
                        <template v-else>
                            <LoginForm />
                        </template>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import RegisterForm from "../components/RegisterForm.vue";
import LoginForm from "../components/LoginForm.vue";

export default {
    name: "Authenticate",
    components: {
        RegisterForm,
        LoginForm,
    },
    computed: {
        displayRegister() {
            return this.$store.state.displayRegister;
        },
    },
};
</script>

<style>
#register {
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
}
.vue-bg {
    background: #1c6f45;
}
hr {
    border-color: #3b556e;
}
</style>
