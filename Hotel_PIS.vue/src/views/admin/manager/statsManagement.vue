<template>
    <h2>Statistiky hotelu</h2>
    <el-row>
        <el-col :span="24" class="stats">
            <div class="stat">
                <RadialProgress :diameter="200" :completed-steps="stayLenght.avarage" :total-steps="stayLenght.max" :innerStrokeColor="'#dcdfe6'" :startColor="'#409eff'" :stopColor="'#409eff'">
                    <p>{{stayLenght.avarage}}</p>
                </RadialProgress>
                <p class="stat-name">Průměrná délka pobytu</p>
            </div>
            <div class="stat">
                <el-icon :size="140" :color="'#dcdfe6'"><user-filled /></el-icon>
                <div>
                    <p>{{mostLoyalClient}}</p>
                    <p class="stat-name">Nejčastějsí host</p>
                </div>
            </div>
            <div class="stat">
                <el-icon :size="140" :color="'#67c23a'"><star /></el-icon>
                <div>
                    <p>Pokoj {{mostBusyRoom.roomNumber}}: {{mostBusyRoom.count}}x</p>
                    <p class="stat-name">Nejčastěji rezervovaný pokoj</p>
                </div>
            </div>
            <div class="stat">
                <el-icon :size="140" :color="'#f56c6c'"><star /></el-icon>
                <div>
                    <p>Pokoj {{leastBusyRoom.roomNumber}}: {{leastBusyRoom.count}}x</p>
                    <p class="stat-name">Nejméně rezervovaný pokoj</p>
                </div>
            </div>
            <div class="stat">
                <el-icon :size="140" :color="'#e6a23c'"><star /></el-icon>
                <div>
                    <p>Pokoj {{mostFailuresRoom.roomNumber}}: {{mostFailuresRoom.count}}x</p>
                    <p class="stat-name">Pokoj s nejvíce závadami</p>
                </div>
            </div>
            <div class="stat contracts">
                <h3>Nejbližší konce smluv</h3>
                <div class="employee" v-for="(employee, index) in employees" :key="index">
                    <p>{{employee.firstName}} {{employee.secondName}}</p>
                    <p>{{employee.contractDueDae}}</p>
                </div>
            </div>
        </el-col>
    </el-row>
</template>
<script lang="js">
    import RadialProgress from "vue3-radial-progress";
    import { ElMessage } from 'element-plus'
    export default {
        components: {
            ElMessage,
            RadialProgress
        },
        data() {
            return {
                employees: [
                    {
                        firstName: "Petr",
                        secondName: "Novák",
                        contractDueDae: "1. 1. 2022"
                    },
                    {
                        firstName: "Petr",
                        secondName: "Novák",
                        contractDueDae: "1. 1. 2022"
                    },
                    {
                        firstName: "Petr",
                        secondName: "Novák",
                        contractDueDae: "1. 1. 2022"
                    },
                    {
                        firstName: "Petr",
                        secondName: "Novák",
                        contractDueDae: "1. 1. 2022"
                    },
                    {
                        firstName: "Petr",
                        secondName: "Novák",
                        contractDueDae: "1. 1. 2022"
                    },
                    {
                        firstName: "Petr",
                        secondName: "Novák",
                        contractDueDae: "1. 1. 2022"
                    }
                ],
                stayLenght: {
                    max: 14,
                    avarage: 0
                },
                mostLoyalClient: null,
                mostBusyRoom: {
                    roomNumber: null,
                    count: null
                },
                leastBusyRoom: {
                    roomNumber: null,
                    count: null
                },
                mostFailuresRoom: {
                    roomNumber: 2222,
                    count: 3
                }
            };
        },
        created() {
            this.loadStats();
            setTimeout(() => this.stayLenght.avarage = 5, 200);
        },
        methods: {
            loadStats() {
                fetch('api/Statistics/GetStatistics', {
                    method: 'GET',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + this.$store.getters.getLoggedUserToken,
                    },
                })
                .then(r => r.json())
                .then(stats => {
                    //TODO stayLength.max!!!
                    setTimeout(() => this.stayLenght.avarage = stats.averageStay, 200);
                    this.mostLoyalClient = stats.mostLoyalClient;
                    this.mostBusyRoom.roomNumber = stats.mostBusyRoomNumber;
                    this.mostBusyRoom.count = stats.mostBusyRoomCount
                    this.leastBusyRoom.roomNumber = stats.notWantedRoomNumber;
                    this.leastBusyRoom.count = stats.notWantedRoomCount;
                    //TODO: mostfailureRoom
                    //TODO: employees
                    ElMessage({ "message": "Statistiky načteny", "type": "success", "custom-class": "message-class" });
                })
                .catch(error => {
                    ElMessage.error({ "message": "Statistiky se nepodařilo načíst!", "custom-class": "message-class", "grouping": true });
                    console.log(error);
                });
            }
        }
    };
</script>
<style scoped>
    h2{
        margin-bottom: 20px;
        text-align: center;
    }
    .stats{
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        justify-content: space-between;
        align-items: flex-start;
    }
    .stat {
        text-align: center;
        flex-basis: 30%;
        width: 30%;
        display: flex;
        flex-direction: column;
        flex-wrap: nowrap;
        justify-content: space-between;
        align-items: center;
        height: 245px;
        margin-bottom: 60px;
    }
    .stat p:not(p.stat-name):not(.employee p){
        font-weight: 500;
        font-size: 30px;
        color: var(--el-color-primary);
    }
    .stat p.stat-name{
        margin-top: 20px;
    }
    .stat .icon{
        height: 200px;
    }
    .contracts{
        text-align: left;
        align-items: flex-start;
        justify-content: flex-start;
        height: unset;
    }
    .contracts .employee{
        display: flex;
        flex-direction: row;
        flex-wrap: nowrap;
        justify-content: space-between;
        width: 100%;
        padding: 10px;
    }
    .contracts .employee:nth-child(odd){
        background: var(--el-color-info-light-9);
    }
    @media screen and (max-width: 992px) {
        .stat {
            flex-basis: 47.5%;
            width: 47.5%;
        }
    }
    @media screen and (max-width: 768px) {
        .stat {
            flex-basis: 100%;
            width: 100%;
            height: unset;
        }
    }
    @media screen and (max-width: 480px) {
        .stat p:not(p.stat-name):not(.employee p){
            font-size: 24px
        }
    }
</style>