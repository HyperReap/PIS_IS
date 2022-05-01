<template>
    <h2 style="text-align: center;">Správa rezervací</h2>
    <el-row v-if="reservations" class="rowCenter">
        <el-col :span="20">
            <div v-if="reservations.length" >
                <table class="table">
                    <thead class="thead">
                        <th>Jméno hosta</th>
                        <th>Email hosta</th>
                        <th>Příjezd</th>
                        <th>Odjezd</th>
                        <th>Pokoj</th>
                        <th>Stav pokoje</th>
                        <th>Zaplaceno</th>
                        <th>Stav rezervace</th>
                        <th class="text-center">Akce</th>
                    </thead>
                    <tr v-for="reservation in reservations" :key="reservations.id">
                        <reservation-item :reservation="reservation" :rooms="rooms" @reservationItem="loadReservations"/>
                    </tr>
                </table>
            </div>
        </el-col>
    </el-row>
    <el-row v-else>
        <p>Žádné reservace k zobrazení!</p>
    </el-row>
</template>

<script lang="js">
    import reservationItem from '@/components/receptionist/reservation-item.vue'
    import { ElMessage } from 'element-plus'

    export default {
        components: {
            reservationItem,
            ElMessage
        },
        data() {
            return {
                reservations: null,
                rooms: null
            };
        },
        created() {
            this.loadReservations();
        },
        computed: {
            
        },
        methods: {
            loadReservations() {
                this.reservations = null;
                this.rooms = null;
                this.$root.loading = !this.$root.loading
                fetch('api/Reservation/GetInProgressReservations', {
                    method: 'GET',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + this.$store.getters.getLoggedUserToken
                    },
                })
                    .then(r => r.json())
                    .then(json => {
                        this.reservations = json;
                        this.$root.loading = !this.$root.loading
                        
                        return;
                    })
                    .catch(error => {
                        this.$root.loading = !this.$root.loading
                        ElMessage.error({ "message": "Nepodařilo se načíst rezervace!", "custom-class": "message-class" });
                        console.log(error);
                    });
                this.$root.loading = !this.$root.loading
                fetch('api/Room/GetAll', {
                    method: 'GET',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + this.$store.getters.getLoggedUserToken
                    },
                })
                    .then(r => r.json())
                    .then(json => {
                        this.rooms = json;
                        this.$root.loading = !this.$root.loading
                        return;
                    })
                    .catch(error => {
                        this.$root.loading = !this.$root.loading
                        ElMessage.error({ "message": "Nepodařilo se načíst rezervaci!", "custom-class": "message-class" });
                        console.log(error);
                    });
            },
            formatDate(date) {
                date = new Date(date);
                return date.getDate() + '. ' + (date.getMonth() + 1) + '. ' + date.getFullYear();
            },
        }
    };
</script>

<style scoped>
    @import '../../assets/bootstrap.min.css';
    .thead {
        color: #fff;
        border-color: #409eff;
        background-color: #409eff;
        font-weight: bold;
        font-size: large;
    }
    .rowCenter {
        margin-top: 25px;
        display: flex;
        align-items: center;
        justify-content: center;
    }
</style>