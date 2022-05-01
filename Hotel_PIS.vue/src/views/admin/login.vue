<template>
    <div class="login">
        <el-card shadow="always">
            <template #header>
                <div class="card-header">
                    <h2>Přihlášení</h2>
                </div>
            </template>
            <div>
                <el-form class="data-form" :model="user" :rules="rules" ref="login">
                    <el-form-item prop="email">
                        <el-input v-model="user.email" placeholder="Email" prefix-icon="UserFilled"></el-input>
                    </el-form-item>
                    <el-form-item prop="password">
                        <el-input v-model="user.password" placeholder="Heslo" type="password" prefix-icon="Lock"></el-input>
                    </el-form-item>
                    <el-form-item>
                        <el-button :loading="loading" class="button" type="primary" @click="login()">Přihlásit se</el-button>
                    </el-form-item>
                </el-form>
            </div>
        </el-card>
    </div>
</template>
<script lang="js">
    import { ElMessage } from 'element-plus'
    export default {
        components: {
            ElMessage,
        },
        data() {
            return {
                user: {
                    email: null,
                    password: null
                },
                loading: false,
                rules: {
                    email: {
                        required: true,
                        message: "Zadejte platný email!",
                        pattern: "[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,63}$",
                        trigger: "blur",
                        autocomplete: "off"
                    },
                    password: {
                        required: true,
                        message: "Zadejte platné heslo!",
                        pattern: "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^0-9a-zA-Z]).{8,15}$",
                        trigger: "blur",
                        min: 8,
                        max: 15,
                        autocomplete: "off"
                    },
                }
            };
        },
        created() {
        },
        methods: {
            login() {
                this.$refs.login.validate((result) => {
                    if (result) {
                        this.loading = !this.loading;
                        fetch('api/Employee/Login', {
                            method: 'POST',
                            headers: {
                                'Accept': 'application/json',
                                'Content-Type': 'application/json'
                            },
                            body: JSON.stringify(this.user)
                        })
                        .then(r => r.json())
                        .then(loggedUser => {
                            if (loggedUser.hasOwnProperty('password')) {
                                delete loggedUser.password;
                            }
                            ElMessage({ "message": "Uživatel přihlášen", "type": "success", "custom-class": "message-class" });
                            this.$store.dispatch('setLoggedUser', loggedUser)
                            this.loading = !this.loading;
                            this.$router.push({ path: '/' });
                            return
                        })
                        .catch(error => {
                            ElMessage.error({ "message": "Přihlášení se nezdařilo!", "custom-class": "message-class", "grouping": true });
                            this.loading = !this.loading;
                            console.log(error);
                        });
                    }
                });
            }
        }
    };
</script>
<style scoped>
    .login {
        display: flex;
        flex-direction: row;
        flex-wrap: nowrap;
        justify-content: center;
        align-items: center;
        height: calc(100vh - 40px);
    }
    .login .el-card {
        width: 300px;
        display: flex;
        flex-direction: column;
        justify-content: center;
    }
    .card-header h2{
        text-align: center;
    }
    .button,
    .button >>> .el-button{
        flex-basis: 100%;
    }
</style>