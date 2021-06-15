<template>
    <form id="signup-form">
        <div class="row">
            <div class="col-12 form-group">
                <label class="col-form-label col-form-label-lg"
                    >Full Name <span class="text-danger">*</span></label
                >
                <input type="text" class="form-control form-control-lg" />
            </div>
            <div class="col-12 form-group">
                <label class="col-form-label col-form-label-lg"
                    >Email <span class="text-danger">*</span></label
                >
                <input type="email" class="form-control form-control-lg" />
            </div>
            <div class="col-12 form-group">
                <label class="col-form-label col-form-label-lg"
                    >Country <span class="text-danger">*</span></label
                >
                <select class="form-control form-control-lg">
                    <option value="">Select Country</option>
                </select>
            </div>
            <div class="col-12 form-group">
                <label class="col-form-label col-form-label-lg"
                    >Password <span class="text-danger">*</span></label
                >
                <input type="password" class="form-control form-control-lg" />
            </div>
            <div class="col-12 form-group text-center">
                <button class="btn btn-vue btn-lg col-4">Sign Up</button>
            </div>
        </div>
    </form>
</template>
<script>
export default {
    name: "Form",
};
</script>
<style>
.btn-vue {
    background: #1c6f45;
    font-weight: bold;
    color: white;
}
</style>
