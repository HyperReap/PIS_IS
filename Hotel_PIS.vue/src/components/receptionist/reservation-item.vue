<template>
    <td>{{reservation.firstName}} {{reservation.secondName}}</td>
    <td>{{reservation.email}}</td>
    <td>{{formatDate(reservation.dateFrom)}}</td>
    <td>{{formatDate(reservation.dateTo)}}</td>
    <td>{{reservation.roomNumber}}</td>

    <td v-if="isRoomCleaned()">
        Uklizen
    </td>
    <td v-else>
        Neuklizen
    </td>

    <td v-if="isPayed()">
        Zaplaceno
    </td>
    <td v-else>
        {{reservation.paid}} Kč / {{reservation.cost}} Kč
    </td>

    <td>{{reservationStateWord(reservation.reservationState)}}</td>
    <td>
        <div class="d-flex">
            <el-popconfirm v-if="reservation.reservationState == 0 && isPayed() && isRoomCleaned()" confirmButtonType="primary" confirm-button-text="Ano" cancel-button-text="Ne" icon-color="blue" title="Opravdu chcete změnit stav rezervace ne check-in?" @confirm="checkIn()">
                <template #reference>
                    <el-button type="primary" class="button button-width">Check-in</el-button>
                </template>
            </el-popconfirm>
            <el-popconfirm v-if="reservation.reservationState == 1 && isPayed()" confirmButtonType="primary" confirm-button-text="Ano" cancel-button-text="Ne" icon-color="blue" title="Opravdu chcete změnit stav rezervace ne check-out?" @confirm="checkOut()">
                <template #reference>
                    <el-button type="primary" class="button button-width">Check-out</el-button>
                </template>
            </el-popconfirm>
            <el-button type="primary" v-if="!isPayed()" class="button button-width" @click="openPaymentGateway()">Zaplatit nedoplatek</el-button>
            <el-popconfirm v-if="reservation.reservationState == 0 && !isPayed()" confirmButtonType="danger" confirm-button-text="Ano" cancel-button-text="Ne" icon-color="red" title="Opravdu chcete zrušit rezervaci?" @confirm="cancelReservation()">
                <template #reference>
                    <el-button type="danger" class="button button-width">Zrušit</el-button>
                </template>
            </el-popconfirm>
        </div>
    </td>

    <el-drawer v-model="drawer" custom-class="drawer-min-width" direction="rtl">
        <h2 class="set-margin-bottom">Platební brána</h2>
        <p>Platba za rezervaci číslo <span class="bold-font">{{payInfo.id}}</span></p>
        <el-form ref="payInfo" :model="payInfo" class="data-form">
            <el-form-item prop="amount" label="Částka">
                <el-input type="number" :disabled="true" v-model.number="payInfo.amount"></el-input>
            </el-form-item>
            <el-form-item class="button">
                <el-button type="primary" @click="payArrear()">Zaplatit nedoplatek</el-button>
            </el-form-item>
        </el-form>
        <img src="../../assets/gopay.png" class="cards">
    </el-drawer>
</template>
<script lang="js">
    import { ElMessage } from 'element-plus'
    export default {
        name: 'ReservationItem',
        props: ['reservation', 'rooms'],
        emits: ['reservationItem'],
        components: {
            ElMessage
        },
        data() {
            return {
                days: ['Ne', 'Po', 'Út', 'St', 'Čt', 'Pá', 'So'],
                reservationStates: ['Rezervováno', 'Check-In', 'Check-Out', 'Zrušená'],
                drawer: false,
                payInfo: {
                    price: null,
                    id: null,
                    amount: null,
                }
            };
        },
        methods: {
            formatDate(date) {
                date = new Date(date);
                return this.days[date.getDay()] + ", " + date.getDate() + '. ' + (date.getMonth() + 1) + '. ' + date.getFullYear();
            },
            reservationStateWord(state) {
                return this.reservationStates[state];
            },
            isRoomCleaned() {
                var flag = false;
                if (this.rooms != null) {
                    this.rooms.forEach(room => {
                        if (room.roomNumber == this.reservation.roomNumber) {
                            flag = room.isCleaned;
                        }
                    })
                }
                return flag;
            },
            checkIn() {
                fetch('api/Reservation/CheckIn?id=' + this.reservation.reservationId, {
                    method: 'GET',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + this.$store.getters.getLoggedUserToken
                    },
                })
                    .then(r => {
                        if (r.status === 200) {
                            ElMessage({ "message": "Stav rezervace změněn na check in!", "type": "success", "custom-class": "message-class" });
                            this.$emit('reservationItem');
                            this.reservation.reservationState = 1;
                        }
                        else {
                            ElMessage.error({ "message": "Nepodařilo se změnit stav rezervace na check in!", "custom-class": "message-class", "grouping": true });
                        }
                        return;
                    })
                    .catch(error => {
                        ElMessage.error({ "message": "Nepodařilo se změnit stav rezervace na check in!", "custom-class": "message-class", "grouping": true });
                        console.log(error);
                    });
            },
            checkOut() {
                fetch('api/Reservation/CheckOut?id=' + this.reservation.reservationId, {
                    method: 'GET',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + this.$store.getters.getLoggedUserToken
                    },
                })
                    .then(r => {
                        if (r.status === 200) {
                            ElMessage({ "message": "Stav rezervace změněn na check out!", "type": "success", "custom-class": "message-class" });
                            this.reservation.reservationState = 2;
                            this.$emit('reservationItem');
                        }
                        else {
                            ElMessage.error({ "message": "Nepodařilo se změnit stav rezervace na check out!", "custom-class": "message-class", "grouping": true });
                        }
                        return;
                    })
                    .catch(error => {
                        ElMessage.error({ "message": "Nepodařilo se změnit stav rezervace na check out!", "custom-class": "message-class", "grouping": true });
                        console.log(error);
                    });
            },
            cancelReservation() {
                fetch('api/Reservation/CancelReservation?id=' + this.reservation.reservationId, {
                    method: 'GET',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + this.$store.getters.getLoggedUserToken
                    },
                })
                    .then(r => {
                        if (r.status === 200) {
                            ElMessage({ "message": "Rezervace byla zrušena!", "type": "success", "custom-class": "message-class" });
                            this.reservation.reservationState = 3;
                            this.$emit('reservationItem');
                        }
                        else {
                            ElMessage.error({ "message": "Nepodařilo se zrušit rezervaci!", "custom-class": "message-class", "grouping": true });
                        }
                        return;
                    })
                    .catch(error => {
                        ElMessage.error({ "message": "Nepodařilo se zrušit rezervaci!", "custom-class": "message-class", "grouping": true });
                        console.log(error);
                    });
            },
            payArrear() {
                fetch('api/Reservation/PayArrear?id=' + this.reservation.reservationId, {
                    method: 'GET',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + this.$store.getters.getLoggedUserToken
                    },
                })
                    .then(r => {
                        if (r.status === 200) {
                            ElMessage({ "message": "Nedoplatek zaplacen!", "type": "success", "custom-class": "message-class" });
                            this.$emit('reservationItem');
                            this.reservation.paid = this.payInfo.amount;
                            this.drawer = false;
                        } else {
                            ElMessage.error({ "message": "Nedoplatek se nepodařilo zaplatit!", "custom-class": "message-class", "grouping": true });
                        }
                        return;
                     })
                     .catch(error => {
                          ElMessage.error({ "message": "Nedoplatek se nepodařilo zaplatit!", "custom-class": "message-class", "grouping": true });
                          console.log(error);
                     });
            },
            isPayed() {
                return (this.reservation.cost == this.reservation.paid)
            },
            openPaymentGateway() {
                this.payInfo.id = this.reservation.id;
                this.payInfo.price = this.reservation.cost;
                this.payInfo.amount = this.reservation.cost - this.reservation.paid;
                this.drawer = true
            },
        }
    };
</script>
<style scoped src="../../assets/bootstrap.min.css"></style>
<style scoped>
    .drawer-min-width {
        min-width: 320px;
    }

    .button-width {
        width: 140px;
    }

</style>