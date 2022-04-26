<template>
    <h2>Potvrzení rezervace</h2>
    <el-row v-for="reservation in reservations" :key="reservation.roomId" class="reservation">
        <el-col :span="8" class="reservation-col-small">
            <div>
                <p class="smaller-letters">Datum příjezdu</p>
                <p class="bigger-letters">{{formatDate(reservation.dateFrom)}}</p>
            </div>
            <div>
                <p class="smaller-letters">Datum odjezdu</p>
                <p class="bigger-letters">{{formatDate(reservation.dateTo)}}</p>
            </div>
            <div>
                <p class="smaller-letters">Délka pobytu</p>
                <p class="bold-font">{{stayLength(reservation.dateFrom, reservation.dateTo)}}</p>
            </div>
            <div>
                <p class="smaller-letters">Počet hostů</p>
                <p class="bold-font">{{reservation.numberOfPeople}} {{customerWord(reservation.numberOfPeople)}}</p>
            </div>
            <div class="final-price">
                <p class="smaller-letters">Celková cena</p>
                <p class="bigger-letters blue-letters">{{reservation.cost}} Kč</p>
            </div>
        </el-col>
        <el-col :span="12" class="reservation-col-big">
            <div>
                <p class="smaller-letters">Host</p>
                <p class="bigger-letters">{{reservation.firstName}} {{reservation.secondName}}</p>
            </div>
            <div class="line">
                <div>
                    <p class="smaller-letters">Email</p>
                    <p class="contact">{{reservation.email}}</p>
                </div>
                <div>
                    <p class="smaller-letters">Telefon</p>
                    <p class="contact">{{reservation.phoneNumber}}</p>
                </div>
            </div>
            <div class="line">
                <div>
                    <p class="smaller-letters">Číslo rezervace</p>
                    <p class="bold-font">{{reservation.reservationId}}</p>
                </div>
                <div>
                    <p class="smaller-letters">Číslo pokoje</p>
                    <p class="bold-font">{{reservation.roomNumber}}</p>
                </div>
            </div>
            <div>
                <p class="smaller-letters">Další informace</p>
                <p class="bold-font">Přijezd do hotelu je možný od 14:00. Při příjezdu nahlaste na recepci číslo rezervace.</p>
            </div>
        </el-col>
        <el-col :span="4" class="reservation-col-buttons" v-if="!reservation.paid">
            <el-button type="primary" class="button" @click="openPaymentGateway(reservation.reservationId, reservation.cost)">Zaplatit zálohu</el-button>
            <el-popconfirm confirmButtonType="danger" confirm-button-text="Ano" cancel-button-text="Ne" icon-color="red" title="Opravdu chcete zručit rezervaci?" @confirm="cancelReservation(reservation.reservationId)">
                <template #reference>
                    <el-button type="danger" class="button">Zrušit rezervaci</el-button>
                </template>
            </el-popconfirm>
        </el-col>
        <el-col :span="4" class="reservation-col-paid" v-else>
            <p>Záloha zaplacena</p>
        </el-col>
    </el-row>
    <el-drawer v-model="drawer" direction="rtl">
        <h2>Platební brána</h2>
        <p>Platba za rezervaci číslo <span class="bold-font">{{payInfo.id}}</span></p>
        <el-form ref="payInfo" :model="payInfo" :rules="rules" class="data-form">
            <el-form-item prop="amount" label="Částka">
                <el-input type="number" :min="1" placeholder="Zadejte částku do výše 33%" :max="payInfo.maxPrice" v-model.number="payInfo.amount"></el-input>
            </el-form-item>
            <el-form-item class="button">
                <el-button type="primary" @click="payDeposit()">Zaplatit zálohu</el-button>
            </el-form-item>
        </el-form>
        <img src="../../assets/gopay.png" class="cards">
    </el-drawer>
</template>
<script lang="js">
    import { ElMessage } from 'element-plus'
    export default {
        components: {
            ElMessage
        },
        data() {
            return {
                drawer: false,
                payInfo: {
                    maxPrice: null,
                    id: null,                
                    amount: null,
                },
                reservations: [],
                days: ['Ne', 'Po', 'Út', 'St', 'Čt', 'Pá', 'So']
            };
        },
        computed: {
            rules() {                
                return {
                    amount: {
                        required: true,
                        type: "number",
                        message: "Zadejte částku do výše max 33%",
                        pattern: "[0-9]+",
                        trigger: "blur",
                        min: 1,
                        max: this.payInfo.maxPrice,
                        autocomplete: "off"
                    }
                };
            },
        },
        created() {
            this.reservations = this.$store.getters.getReservationDetails;
            this.$store.dispatch('clearReservationsDetails', []);
            if (!this.reservations.length) {
                this.$router.push({ path: '/rezervace' });
            }
        },
        methods: {
            stayLength(dateFrom, dateTo) {
                let diff = new Date(dateTo) - new Date(dateFrom)
                diff = diff / (1000 * 60 * 60 * 24);
                let word;
                switch (true) {
                    case (diff > 4):
                        word = "dnů"
                        break;
                    case (diff == 1):
                        word = "den"
                        break;
                    case (diff >= 2 && diff <= 4):
                        word = "dny"
                        break;
                    default:
                        word = "dnů"
                }
                return (diff + ' ' + word);
            },
            customerWord(numberOfCustomers) {
                let word;
                switch (true) {
                    case (numberOfCustomers > 4):
                        word = "hostů"
                        break;
                    case (numberOfCustomers == 1):
                        word = "host"
                        break;
                    case (numberOfCustomers >= 2 && numberOfCustomers <= 4):
                        word = "hosté"
                        break;
                    default:
                        word = "hostů"
                }
                return word
            },
            formatDate(date) {
                date = new Date(date);
                return this.days[date.getDay()] + ", " + date.getDate() + '. ' + (date.getMonth() + 1) + '. ' + date.getFullYear();
            },
            openPaymentGateway(id, maxPrice) {
                this.payInfo.id = id;
                this.payInfo.maxPrice = Math.ceil(maxPrice / 3);
                this.payInfo.amount = Math.ceil(maxPrice / 3);
                this.drawer = true
            },
            payDeposit() {
                this.$refs.payInfo.validate((result) => {
                    if (result) {
                        let requestParams = this.$createRequestParams(this.payInfo, true);
                        fetch('api/Reservation/PayDeposit' + requestParams, {
                            method: 'GET',
                            headers: {
                                'Accept': 'application/json',
                                'Content-Type': 'application/json'
                            },
                        })
                        .then(r => {
                            if (r.status === 200) {
                                ElMessage({ "message": "Záloha zaplacena!", "type": "success", "custom-class": "message-class" });
                                let index = this.reservations.findIndex(reservation => {return reservation.reservationId === this.payInfo.id;});
                                this.reservations[index].paid = true;
                                this.drawer = false;
                            }
                            else {
                                ElMessage.error({ "message": "Zálohu se nepodařilo zaplatit!", "custom-class": "message-class", "grouping": true });
                            }
                            return;
                        })
                        .catch(error => {
                            ElMessage.error({ "message": "Zálohu se nepodařilo zaplatit!", "custom-class": "message-class", "grouping": true });
                            console.log(error);
                        });
                    }
                });
            },
            cancelReservation(id) {
                fetch('api/Reservation/CancelReservation?id=' + id, {
                    method: 'GET',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                })
                .then(r => {
                    if (r.status === 200) {
                        ElMessage({ "message": "Rezervace zrušena!", "type": "success", "custom-class": "message-class" });
                        this.reservations = this.reservations.filter(function (reservation) { return reservation.reservationId !== id; });
                        if (!this.reservations.length) {
                            this.$router.push({ path: '/rezervace' });
                        }
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
            }
        }
    }
</script>
<style scoped>
    h2{
        margin-bottom:20px;
    }
    .reservation {
        border-top: 3px solid var(--el-border-color);
        border-right: 1px solid var(--el-border-color);
        border-bottom: 1px solid var(--el-border-color);
        border-left: 1px solid var(--el-border-color);
        margin-bottom: 20px;
        padding: 20px;
    }
    .reservation-col-small {
        border-right: 1px solid var(--el-border-color);
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
    }
    .reservation-col-big {
        padding-left: 20px;
    }
    .smaller-letters {
        font-size: 12px;
        color: var(--el-text-color-secondary);
    }
    .bigger-letters{
        font-size: 24px;
        font-weight: 500;
    }
    .reservation-col-small div{
        flex-basis: 50%;
    }
    .reservation-col-big div{
        margin-bottom: 20px;
    }
    .blue-letters {
        color: var(--el-color-primary)
    }
    .bold-font{
        font-weight: 500;
        font-size: 18px;
    }
    .contact {
        color: var(--el-color-primary);
        font-weight: 500;
    }
    div.line div{
        margin-bottom: 0;
        margin-right: 40px;
    }
    div.line{
        display: flex;
        flex-direction: row;
        flex-wrap: nowrap;
        justify-content: flex-start;
    }
    .final-price{
        margin-top: 50px;
    }
    .reservation-col-buttons{
        display: flex;
        flex-direction: column;
    }
    .reservation-col-buttons .button{
        width: 100%;
        margin: 0 0 20px 0;
    }
    span.bold-font{
        font-size: unset;
    }
    .data-form{
        margin-top: 20px;
    }
    .data-form >>> input::-webkit-outer-spin-button,
    .data-form >>> input::-webkit-inner-spin-button {
        -webkit-appearance: none;
        margin: 0;
    }
    .data-form >>> input[type=number] {
        -moz-appearance: textfield;
    }
    .data-form >>> .el-form-item{
        flex-direction: column;
    }
    .data-form >>> .el-form-item__label{
        text-align: left;
    }
    .cards{
        position: absolute;
        bottom: 20px;
        left: 20px;
        max-width: 100%;
    }
    .data-form .button,
    .data-form .button >>> .el-button {
        flex-basis: 100%;
    }
    .reservation-col-paid p{
        margin-top: 14px;
        color: var(--el-color-primary);
        font-weight: 500;
        font-size: 24px;
        text-align: right
    }
</style>