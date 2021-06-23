<template>
    <div>
        <form id="login-form" v-on:submit.prevent="submit">
            <div class="row">
                <div class="col-12 form-group">
                    <label class="col-form-label col-form-label-lg"
                        >Login</label
                    >
                    <input
                        type="text"
                        v-model.trim="$v.form.login.$model"
                        class="form-control form-control-lg"
                        :class="{
                            'is-invalid': validationStatus($v.form.login),
                        }"
                    />
                    <div
                        v-if="!$v.form.login.required"
                        class="invalid-feedback"
                    >
                        The login field is required.
                    </div>
                </div>
                <div class="col-12 form-group">
                    <label class="col-form-label col-form-label-lg"
                        >Password</label
                    >
                    <input
                        type="password"
                        v-model.trim="$v.form.password.$model"
                        class="form-control form-control-lg"
                        :class="{
                            'is-invalid': validationStatus($v.form.password),
                        }"
                    />
                    <div
                        v-if="!$v.form.password.required"
                        class="invalid-feedback"
                    >
                        The password field is required.
                    </div>
                </div>
                <div class="col-12 form-group text-center">
                    <input
                        type="submit"
                        class="btn btn-vue btn-lg col-4"
                        value="Login"
                        :disabled="this.isDisabled"
                    />
                </div>
            </div>
        </form>
        <hr class="my-4" />
        <div class="col-12 form-group text-center">
            <input
                type="button"
                class="btn btn-vue btn-lg col-4"
                value="Register"
                @click="showRegister"
            />
        </div>
    </div>
</template>
<script>
import { required } from "vuelidate/lib/validators";
export default {
    name: "LoginForm",
    data() {
        return {
            form: {
                login: "",
                password: "",
            },
        };
    },
    computed: {
        isDisabled() {
            return this.$v.$invalid;
        },
    },
    validations: {
        form: {
            login: { required },
            password: { required },
        },
    },
    methods: {
        showRegister() {
            this.$store.state.displayRegister = true;
        },
        resetData() {
            this.form.login = "";
            this.form.password = "";
        },
        validationStatus(validation) {
            return typeof validation != "undefined" ? validation.$error : false;
        },
        submit() {
            this.$v.$touch();
            if (this.$v.$pendding || this.$v.$error) {
                return;
            }
            this.$http
                .post("api/login", {
                    username: this.form.login,
                    password: this.form.password,
                })
                .then((res) => {
                    if (res.status == 200) {
                        const token = res.data.token;
                        localStorage.setItem("user-token", token);
                        localStorage.setItem("current-user", this.form.login);
                        this.$router.push("/game");
                        this.resetData();
                    }
                })
                .catch((e) => {
                    localStorage.removeItem("user-token");
                    localStorage.removeItem("current-user");
                    if (e.response.status == 400) {
                        console.log("TODO wrong login/pass");
                    }
                });
        },
    },
};
</script>
<style>
.btn-vue {
    background: #53b985;
    color: #31485d;
    font-weight: bold;
}
</style>
