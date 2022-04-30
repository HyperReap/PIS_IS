<template>
    <el-button type="primary" @click="openDialog()">Přidat zaměstnance</el-button>
    <el-dialog v-model="dialogVisible" title="Přidat nového zaměstnance" custom-class="employee-dialog">
        <el-form :model="employee" :rules="rules" ref="employee" class="new-employee-form">
            <el-form-item prop="firstName">
                <el-input type="text" placeholder="Jméno" v-model="employee.firstName"></el-input>
            </el-form-item>
            <el-form-item prop="secondName">
                <el-input type="text" placeholder="Příjmení" v-model="employee.secondName"></el-input>
            </el-form-item>
            <el-form-item prop="email">
                <el-input type="email" placeholder="Email" v-model="employee.email"></el-input>
            </el-form-item>
            <el-form-item prop="password">
                <el-input v-model="employee.password" placeholder="Heslo" type="password" class="password" prefix-icon="Lock"></el-input>
            </el-form-item>
            <el-form-item prop="dateFrom">
                <el-date-picker v-model="employee.contractDueDae" type="date" placeholder="Smlouva do" popper-class="date-dropdown"
                                class="date-picker" :disabled-date="disabledDateFrom" format="DD. MM. YYYY" value-format="YYYY-MM-DD" />
            </el-form-item>
            <el-form-item prop="roleId">
                <el-select v-model="employee.roleId" placeholder="Typ zaměstnance" class="role-select">
                    <el-option label="Recepční" value="2"></el-option>
                    <el-option label="Technik" value="3"></el-option>
                    <el-option label="Uklízečka" value="4"></el-option>
                </el-select>
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button type="primary" @click="add()">Přidat zaměstnance</el-button>
            </span>
        </template>
    </el-dialog>
</template>
<script lang="js">
    import { ElMessage } from 'element-plus'
    export default {
        name: 'newEmployee',
        emits: ['newEmployee'],
        components: {
            ElMessage,
        },
        data() {
            return {
                dialogVisible: false,
                dayDuration: 86400000,
                employee: {
                    firstName: null,
                    secondName: null,
                    email: null,
                    password: null,
                    contractDueDae: null,
                    roleId: null,
                },
                rules: {
                    firstName: {
                        required: true,
                        message: "Vyplňte celé jméno!",
                        pattern: "^[A-Za-zÀ-ž]+$",
                        trigger: "blur",
                        min: 3,
                        max: 60,
                        autocomplete: "off"
                    },
                    secondName: {
                        required: true,
                        message: "Vyplňte celé příjmení!",
                        pattern: "^[A-Za-zÀ-ž]+$",
                        trigger: "blur",
                        min: 3,
                        max: 60,
                        autocomplete: "off"
                    },
                    email: {
                        required: true,
                        message: "Zadejte platný email!",
                        pattern: "[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,63}$",
                        trigger: "blur",
                        autocomplete: "off"
                    },
                    contractDueDae: {
                        type: "date",
                        required: false,
                        message: "Smlouva do!",
                        trigger: "blur",
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
                    roleId: {
                        required: true,
                        message: "Vyberte typ zaměstnance!",
                        trigger: "change",
                    },
                }
            };
        }, 
        methods: {
            openDialog() {
                this.dialogVisible = !this.dialogVisible;
            },
            disabledDateFrom(date) {
                return date.getTime() < Date.now() - this.dayDuration
            },
            add() {
                this.$refs.employee.validate((result) => {
                    if (result) {
                        fetch('api/Employee/Save', {
                            method: 'POST',
                            headers: {
                                'Accept': 'application/json',
                                'Content-Type': 'application/json',
                                'Authorization': 'Bearer ' + this.$store.getters.getLoggedUserToken
                            },
                            body: JSON.stringify(this.employee)
                        })
                        .then(r => r.json())
                        .then(json => {
                            this.dialogVisible = !this.dialogVisible;
                            ElMessage({ "message": "Zaměstnanec přidán", "type": "success", "custom-class": "message-class", "grouping": true });
                            if ("id" in json && "firstName" in json && "secondName" in json) {
                                this.$emit('newEmployee', json);
                                this.$refs.employee.resetFields();
                            }
                            else {
                                ElMessage({ "message": "Aktualizujte stránku pro nová data", "type": "success", "custom-class": "message-class", "grouping": true });
                            }
                            return
                        })
                        .catch(error => {
                            this.dialogVisible = !this.dialogVisible;
                            ElMessage.error({ "message": "Nepodařilo se přidat zaměstnance!", "custom-class": "message-class", "grouping": true });
                            console.log(error);
                        });
                    }
                });
            }
        }
    }
</script>
<style scoped>
    .new-employee-form >>> .date-picker,
    .new-employee-form >>> .role-select
    {
        width: 100% !important;
    }
    .new-employee-form >>> .date-picker input,
    .new-employee-form >>> .password input
    {
        padding-left: 30px!important;
    }
</style>