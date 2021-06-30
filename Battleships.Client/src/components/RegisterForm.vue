<template>
    <div>
        <form id="register-form" v-on:submit.prevent="submit">
            <div class="row">
                <div class="col-12 form-group">
                    <label class="col-form-label col-form-label-lg"
                        >Login <span class="text-danger">*</span></label
                    >
                    <input
                        type="text"
                        v-model.trim="$v.form.login.$model"
                        :class="{
                            'is-invalid': validationStatus($v.form.login),
                        }"
                        class="form-control form-control-lg"
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
                        >Password <span class="text-danger">*</span></label
                    >
                    <input
                        type="password"
                        v-model.trim="$v.form.password.$model"
                        :class="{
                            'is-invalid': validationStatus($v.form.password),
                        }"
                        class="form-control form-control-lg"
                    />
                    <div
                        v-if="!$v.form.password.required"
                        class="invalid-feedback"
                    >
                        The password field is required.
                    </div>
                    <div
                        v-if="!$v.form.password.minLength"
                        class="invalid-feedback"
                    >
                        You must have at least
                        {{ $v.form.password.$params.minLength.min }} letters.
                    </div>
                    <div
                        v-if="!$v.form.password.maxLength"
                        class="invalid-feedback"
                    >
                        You must not have greater then
                        {{ $v.form.password.$params.maxLength.min }} letters.
                    </div>
                </div>
                <div class="col-12 form-group">
                    <label class="col-form-label col-form-label-lg"
                        >Confirm Password
                        <span class="text-danger">*</span></label
                    >
                    <input
                        type="password"
                        v-model.trim="$v.form.confirmPassword.$model"
                        :class="{
                            'is-invalid': validationStatus(
                                $v.form.confirmPassword
                            ),
                        }"
                        class="form-control form-control-lg"
                    />
                    <div
                        v-if="!$v.form.confirmPassword.required"
                        class="invalid-feedback"
                    >
                        The confirm password field is required.
                    </div>
                    <div
                        v-if="!$v.form.confirmPassword.sameAsPassword"
                        class="invalid-feedback"
                    >
                        You must confirm your password.
                    </div>
                </div>
                <div class="col-12 form-group text-center">
                    <input
                        type="submit"
                        class="btn btn-vue btn-lg col-4"
                        value="Register"
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
                value="Back"
                @click="showLogin"
            />
        </div>
    </div>
</template>
<script>
import {
    required,
    minLength,
    maxLength,
    sameAs,
} from "vuelidate/lib/validators";
export default {
    name: "RegisterForm",
    data() {
        return {
            form: {
                login: "",
                password: "",
                confirmPassword: "",
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
            password: {
                required,
                minLength: minLength(8),
                maxLength: maxLength(18),
            },
            confirmPassword: {
                required,
                sameAsPassword: sameAs("password"),
            },
        },
    },
    methods: {
        showLogin() {
            this.$store.state.displayRegister = false;
        },
        validationStatus(validation) {
            return typeof validation != "undefined" ? validation.$error : false;
        },
        resetData() {
            this.form.login = "";
            this.form.password = "";
            this.form.confirmPassword = "";
        },
        submit() {
            this.$v.$touch();
            if (this.$v.$pendding || this.$v.$error) {
                return;
            }

            this.$http
                .post(
                    "api/register",
                    {
                        username: this.form.login,
                        password: this.form.password,
                    },
                    {
                        headers: {
                            "Content-Type": "application/json; charset=utf-8",
                        },
                    }
                )
                .then((res) => {
                    console.log(res.data);
                    this.$store.state.displayRegister = false;
                })
                .catch((e) => {
                    console.log(e);
                });
            this.$v.$reset();
            this.resetData();
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
.btn-vue:disabled {
    pointer-events: none;
}
</style>
