<template>
    <newEmployee @newEmployee="newEmployee" />
    <el-row v-if="employees">
        <el-col :span="24" class="employees">
            <el-card shadow="hover" v-for="employee in employees" :class="['emploeeRole' + employee.roleId, 'employee']" :key="employee.id" @click="openEmployeeDialog(employee)">
                <div class="left">
                    <el-avatar class="user-avatar"> {{initials(employee)}} </el-avatar>
                </div>
                <div class="right">
                    <p class="name">{{ employee.firstName }} {{ employee.secondName }}</p>
                    <p> {{ employee.email }} </p>
                    <p> Smlouva do: {{contractTo(employee.contractDueDae)}}</p>
                </div>
            </el-card>
        </el-col>
    </el-row>
    <el-dialog v-model="dialogVisible" custom-class="new-employee-dialog">
        <h3>Upravit údaje</h3>
        <el-form :model="currentEmployee" :rules="rules" ref="update" class="new-employee-form">
            <el-form-item prop="firstName">
                <el-input type="text" placeholder="Jméno" v-model="currentEmployee.firstName"></el-input>
            </el-form-item>
            <el-form-item prop="secondName">
                <el-input type="text" placeholder="Příjmení" v-model="currentEmployee.secondName"></el-input>
            </el-form-item>
            <el-form-item prop="dateFrom" v-if="currentEmployee.roleId != 1">
                <el-date-picker v-model="currentEmployee.contractDueDae" type="date" placeholder="Smlouva do" popper-class="date-dropdown"
                                class="date-picker" :disabled-date="disabledDateFrom" format="DD. MM. YYYY" value-format="YYYY-MM-DD" />
            </el-form-item>
            <el-form-item prop="roleId" v-if="currentEmployee.roleId != 1">
                <el-select v-model="currentEmployee.roleId" placeholder="Typ zaměstnance" class="role-select">
                    <el-option v-for="role in roles" :label="role.label" :key="role.roleId" :value="role.roleId" value-key="role.roleId">
                    </el-option>
                </el-select>
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button type="primary" @click="updateEmployee(currentEmployee.id)">Aktualizovat</el-button>
                <el-button type="danger" v-if="currentEmployee.roleId != 1" @click="deleteEmployee(currentEmployee.id)">Odstranit</el-button>
            </span>
        </template>
    </el-dialog>
</template>
<script lang="js">
    import newEmployee from "@/components/admin/manager/newEmployee.vue";
    import { ElMessage } from 'element-plus'
    export default {
        components: {
            newEmployee,
            ElMessage
        },
        data() {
            return {
                dayDuration: 86400000,
                employees: [],
                currentEmployee: {
                    firstName: null,
                    secondName: null,
                    email: null,
                    password: null,
                    contractDueDae: null,
                    roleId: null,
                    id: null
                },
                roles: [
                    {
                        label: "Recepční",
                        roleId: 2
                    },
                    {
                        label: "Technik",
                        roleId: 3
                    },
                    {
                        label: "Uklízečka",
                        roleId: 4
                    }
                ],
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
                    contractDueDae: {
                        type: "date",
                        required: false,
                        message: "Smlouva do!",
                        trigger: "blur",
                    },
                    roleId: {
                        required: true,
                        message: "Vyberte typ zaměstnance!",
                        trigger: "change",
                    },
                },
                dialogVisible: false
            };
        },
        created() {
            this.loadEmployees();
        },
        methods: {
            disabledDateFrom(date) {
                return date.getTime() < Date.now() - this.dayDuration
            },
            loadEmployees() {
                this.employees = [];
                this.$root.loading = !this.$root.loading
                fetch('api/Employee/GetAll', {
                    method: 'GET',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + this.$store.getters.getLoggedUserToken,
                    },
                })
                .then(r => r.json())
                .then(json => {
                    this.employees = json;
                    this.$root.loading = !this.$root.loading
                    ElMessage({ "message": "Zaměstnanci načteni", "type": "success", "custom-class": "message-class" });
                })
                .catch(error => {
                    this.$root.loading = !this.$root.loading
                    ElMessage.error({ "message": "Nepodařilo se načíst zaměstnance!", "custom-class": "message-class", "grouping": true });
                    console.log(error);
                });
            },
            initials(employee) {
                return employee.firstName[0] + employee.secondName[0]
            },
            contractTo(date) {
                if (date === null) {
                    return "neomezeně"
                }
                else {
                    date = new Date(date);
                    return date.getDate() + '. ' + (date.getMonth() + 1) + '. ' + date.getFullYear();
                }
            },
            openEmployeeDialog(employee) {
                this.currentEmployee = employee;
                this.dialogVisible = true;
            },
            newEmployee(employee) {
                this.employees.push(employee);
            },
            deleteEmployee(id) {
                this.dialogVisible = false;
                fetch('api/Employee/Delete?id=' + id, {
                    method: 'DELETE',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + this.$store.getters.getLoggedUserToken,
                    },
                })
                .then(r => r.json())
                .then(response => {
                    if (response === true) {
                        ElMessage({ "message": "Zaměstnanec odebrán!", "type": "success", "custom-class": "message-class" });
                        this.employees = this.employees.filter(function (employee) { return employee.id !== id; });
                    }
                    else {
                        ElMessage.error({ "message": "Nepodařilo se odstranit zaměstnance!", "custom-class": "message-class", "grouping": true });
                    }
                    return;
                })
                .catch(error => {
                    ElMessage.error({ "message": "Nepodařilo se odstranit zaměstnance!", "custom-class": "message-class", "grouping": true });
                    console.log(error);
                });
            },
            updateEmployee(id) {
                this.$refs.update.validate((result) => {
                    if (result) {
                        fetch('api/Employee/Save?id=' + id, {
                            method: 'POST',
                            headers: {
                                'Accept': 'application/json',
                                'Content-Type': 'application/json',
                                'Authorization': 'Bearer ' + this.$store.getters.getLoggedUserToken,
                            },
                            body: JSON.stringify(this.currentEmployee)
                        })
                        .then(r => r.json())
                        .then(json => {
                            this.dialogVisible = !this.dialogVisible;
                            ElMessage({ "message": "Zaměstnanec aktualizován", "type": "success", "custom-class": "message-class", "grouping": true });
                            if ("id" in json && "firstName" in json && "secondName" in json) {
                                let index = this.employees.findIndex(employee => { return employee.id === id; });
                                this.employees[index] = json;
                            }
                            else {
                                ElMessage({ "message": "Aktualizujte stránku pro nová data", "type": "success", "custom-class": "message-class", "grouping": true });
                            }
                            return
                        })
                        .catch(error => {
                            this.dialogVisible = !this.dialogVisible;
                            ElMessage.error({ "message": "Nepodařilo se aktualizovat zaměstnance!", "custom-class": "message-class", "grouping": true });
                            console.log(error);
                        });
                    }
                });
            },
        }
    }
</script>
<style scoped>
    .employees {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        justify-content: flex-start;
        margin-top: 20px;
    }

    .employee {
        border: 1px solid red;
        width: 30%;
        flex-basis: 30%;
        margin-bottom: 20px;
        border: 1px solid var(--el-border-color);
        margin-right: 5%;
    }

    .employee:hover {
        border: 1px solid var(--el-color-primary);
        cursor: pointer;
    }
    .employee.emploeeRole2:hover{
        border: 1px solid var(--el-color-success);
    }
    .employee.emploeeRole3:hover{
         border: 1px solid var(--el-color-warning);
    }
    .employee.emploeeRole4:hover{
         border: 1px solid var(--el-color-danger);
    }
    .employee.emploeeRole2 .user-avatar{
        background: var(--el-color-success)!important;
    }
    .employee.emploeeRole3 .user-avatar{
         background: var(--el-color-warning)!important;
    }
    .employee.emploeeRole4 .user-avatar{
         background: var(--el-color-danger)!important;
    }
    .employee:nth-child(3n) {
        margin-right: 0;
    }

    .employee p {
        text-overflow: ellipsis;
        overflow: hidden;
        width: 100%;
        white-space: nowrap;
    }
    h3 {
        font-weight: 500;
        margin-bottom: 20px;
        font-size: 18px;
    }
    .left {
        display: flex;
        flex-direction: column;
        justify-content: center;
    }

    .right {
        padding-left: 20px;
    }

    .name {
        font-weight: 500;
        margin-bottom: 5px;
    }
    .employee >>> .el-card__body{
        display: flex;
        flex-direction: row;
        flex-wrap: nowrap;
        justify-content: flex-start;
        align-items: center;
    }
    .new-employee-form >>> .date-picker,
    .new-employee-form >>> .role-select
    {
        width: 100% !important;
    }
    .new-employee-form >>> .date-picker input
    {
        padding-left: 30px!important;
    }
    .new-employee-form {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        justify-content: space-between;
    }
    .new-employee-form >>> .el-form-item{
        width: 47.5%;
        flex-basis: 47.5%;
    }

    @media screen and (max-width: 992px) {
        .employees {
            justify-content: space-between
        }

        .employee {
            width: 45%;
            flex-basis: 45%;
            margin-right: 0;
        }
        .new-employee-form >>> .el-form-item {
            width: 100%;
            flex-basis: 100%;
        }
    }

    @media screen and (max-width: 630px) {
        .employee {
            width: 100%;
            flex-basis: 100%;
        }
    }
</style>