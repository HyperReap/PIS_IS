<template>
    <newEmployee @newEmployee="newEmployee" />
    <el-row v-if="employees">
        <el-col :span="24" class="employees">
            <el-card shadow="hover" v-for="employee in employees" :class="['emploeeRole' + employee.roleId, 'employee']" :key="employee.id" @click="updateEmployee(employee)">
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
        <h3>Test</h3>
        TODO - update user
        <ul>
            <li>Role</li>
            <li>FirstName</li>
            <li>SecondName</li>
            <li>Email</li>
            <li>Contract</li>
        </ul>
        <template #footer>
            <span class="dialog-footer">
                <el-button type="primary">Aktualizovat záznam</el-button>
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
                dialogVisible: false
            };
        },
        created() {
            this.loadEmployees();
        },
        methods: {
            loadEmployees() {
                this.employees = [];
                this.$root.loading = !this.$root.loading
                fetch('api/Employee/GetAll', {
                    method: 'GET',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + this.$store.getters.getLoggedUserToken ,
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
                    return "neomezená doba"
                }
                else {
                    date = new Date(date);
                    return date.getDate() + '. ' + (date.getMonth() + 1) + '. ' + date.getFullYear();
                }
            },
            /*openEmployeeDialog(employee) {
                this.currentEmployee = employee;
                this.dialogVisible = true;
            },
            updateEmployee(id) {
                this.dialogVisible = false;
                fetch('api/Failure/Solve?failureId=' + id, {
                    method: 'GET',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                })
                    .then(r => {
                        if (r.status === 200) {
                            ElMessage({ "message": "Vyřešeno!", "type": "success", "custom-class": "message-class" });
                            this.failures = this.failures.filter(function (failure) { return failure.id !== id; });
                        }
                        else {
                            ElMessage.error({ "message": "Nepodařilo se označit jako vyřešeno!", "custom-class": "message-class", "grouping": true });
                        }
                        return;
                    })
                    .catch(error => {
                        ElMessage.error({ "message": "Nepodařilo se označit jako vyřešeno!", "custom-class": "message-class", "grouping": true });
                        console.log(error);
                    });
            },*/
            newEmployee(employee) {
                this.employees.push(employee);
            }
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

    @media screen and (max-width: 992px) {
        .employees {
            justify-content: space-between
        }

        .employee {
            width: 45%;
            flex-basis: 45%;
            margin-right: 0;
        }
    }

    @media screen and (max-width: 630px) {
        .employee {
            width: 100%;
            flex-basis: 100%;
        }
    }
</style>