<template>
    <h2>Moje rezervace</h2>
    <el-row>
        <el-col :span="8" class="screen-size-edit">
            <el-form ref="clientInfo" :model="clientInfo" :rules="clientInfoRules" class="data-form email-form">
                <el-form-item prop="email">
                    <el-input type="email" placeholder="Email" v-model="clientInfo.email"></el-input>
                </el-form-item>
                <el-form-item class="button">
                    <el-button type="primary" @click="validateEmail()">Načíst rezervace</el-button>
                </el-form-item>
            </el-form>
        </el-col>
    </el-row>
    <el-row v-if="reservations.length" v-for="reservation in reservations" :key="reservation.roomId" class="reservation">
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
                <div>
                    <p class="smaller-letters">Stav rezervace</p>
                    <p class="bold-font">{{reservationStateWord(reservation.reservationState)}}</p>
                </div>
            </div>
            <div v-if="reservation.reservationState == 0">
                <p class="smaller-letters">Další informace</p>
                <p class="bold-font">Přijezd do hotelu je možný od 14:00. Při příjezdu nahlaste na recepci číslo rezervace.</p>
            </div>
        </el-col>
        <el-col :span="4" class="reservation-col-buttons" v-if="reservation.paid == 0 && reservation.reservationState == 0">
            <el-button type="primary" class="button" @click="openPaymentGateway(reservation.reservationId, reservation.cost)">Zaplatit zálohu</el-button>
            <el-popconfirm confirmButtonType="danger" confirm-button-text="Ano" cancel-button-text="Ne" icon-color="red" title="Opravdu chcete zručit rezervaci?" @confirm="cancelReservation(reservation.reservationId)">
                <template #reference>
                    <el-button type="danger" class="button">Zrušit rezervaci</el-button>
                </template>
            </el-popconfirm>
        </el-col>
        <el-col :span="4" class="reservation-col-canceled" v-else-if="reservation.reservationState == 3">
            <p>Rezervace zrušena</p>
        </el-col>
        <el-col :span="4" class="reservation-col-paid" v-else>
            <p>Zaplaceno {{reservation.paid}}&nbsp;Kč</p>
        </el-col>
    </el-row>
    <el-row v-else>
        <p>Žádné rezervace k zobrazení</p>
    </el-row>
    <el-drawer v-model="drawer" custom-class="drawer-min-width" direction="rtl">
        <h2 class="set-margin-bottom">Platební brána</h2>
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
                clientInfo: {
                    email: null
                },
                drawer: false,
                payInfo: {
                    maxPrice: null,
                    id: null,
                    amount: null,
                },
                reservations: [],
                days: ['Ne', 'Po', 'Út', 'St', 'Čt', 'Pá', 'So'],
                reservationStates: ['Rezervováno', 'Check-In', 'Check-Out', 'Zrušená'],
                clientInfoRules: {
                    email: {
                        required: true,
                        message: "Zadejte platný email!",
                        pattern: "[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,63}$",
                        trigger: "blur",
                        autocomplete: "off"
                    }
                }                
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
            this.clientInfo.email = this.$store.getters.getCustomerEmail;
            if (this.clientInfo.email) {
                this.loadReservations();
            }
        },
        methods: {
            reservationStateWord(state) {
                return this.reservationStates[state];
            },
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
                                    let index = this.reservations.findIndex(reservation => { return reservation.reservationId === this.payInfo.id; });
                                    this.reservations[index].paid = this.payInfo.amount;
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
                            let index = this.reservations.findIndex(reservation => { return reservation.reservationId === id; });
                            this.reservations[index].reservationState = 3;
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
            validateEmail() {
                this.$refs.clientInfo.validate((result) => {
                    if (result) {
                        this.loadReservations();
                    }
                });
            },
            loadReservations() {                
                fetch('api/Reservation/GetReservationsByEmail?email=' + encodeURIComponent(this.clientInfo.email), {
                    method: 'GET',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    }
                })
                .then(r => r.json())
                .then(json => {
                    this.reservations = json;
                    if (this.reservations.length) {
                        ElMessage({ "message": "Rezervace načteny!", "type": "success", "custom-class": "message-class", "grouping": true});
                    }
                    else {
                        ElMessage.error({ "message": "Nenalezeny žádné rezervace spjaté se zadaným emailem!", "custom-class": "message-class", "grouping": true });
                    }
                })
                .catch(error => {
                    ElMessage.error({ "message": "Nepodařilo se načíst rezervace!", "custom-class": "message-class", "grouping": true });
                    console.log(error);
                });                    
            }
        }
    }
</script>
<style scoped>
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

    .bigger-letters {
        font-size: 24px;
        font-weight: 500;
    }

    .reservation-col-small div {
        flex-basis: 50%;
    }

    .reservation-col-big div {
        margin-bottom: 20px;
    }

    .blue-letters {
        color: var(--el-color-primary)
    }

    .bold-font {
        font-weight: 500;
        font-size: 18px;
    }

    .contact {
        color: var(--el-color-primary);
        font-weight: 500;
    }

    div.line div {
        margin-bottom: 0;
        flex-basis: 37.5%;
    }
    div.line div:last-child{
        flex-basis: unset;
    }
    div.line {
        display: flex;
        flex-direction: row;
        flex-wrap: nowrap;
        justify-content: flex-start;
    }

    .final-price {
        margin-top: 50px;
    }

    .reservation-col-buttons {
        display: flex;
        flex-direction: column;
    }

    .reservation-col-buttons .button {
        width: 100%;
        margin: 0 0 20px 0;
    }

    span.bold-font {
        font-size: unset;
    }

    .data-form {
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

    .data-form >>> .el-form-item {
        flex-direction: column;
    }

    .data-form >>> .el-form-item__label {
        text-align: left;
    }

    .cards {
        position: absolute;
        bottom: 20px;
        left: 20px;
        max-width: 90%;
    }

    .data-form .button,
    .data-form .button >>> .el-button {
        flex-basis: 100%;
    }

    .reservation-col-paid p {
        margin-top: 14px;
        color: var(--el-color-primary);
        font-weight: 500;
        font-size: 24px;
        text-align: right
    }
    .reservation-col-canceled p {
        margin-top: 14px;
        color: var(--el-color-danger);
        font-weight: 500;
        font-size: 24px;
        text-align: right
    }
    .email-form {
        display: flex;
        flex-direction: row;
        flex-wrap: nowrap;
        justify-content: space-between;
    }
    .email-form .button, 
    .email-form .button .el-button{
        flex-basis: 30%;
    }
    .email-form >>> .el-form-item.is-required{
        flex-basis: 60%;
    }
    .set-margin-bottom{
        margin-bottom: 20px;
    }
    @media screen and (max-width: 1410px) {
        .reservation-col-paid p,
        .reservation-col-canceled p,
        .bigger-letters {
            font-size: 20px;
        }
    }
    @media screen and (max-width: 1200px) {
        .screen-size-edit {
            max-width: 41.6666666667%;
            flex: 0 0 41.6666666667%;
        }
        div.line:nth-child(2){
            flex-direction: column
        }
        div.line:nth-child(2) div:nth-child(2){
            margin-top: 20px;
        }
    }
    @media screen and (max-width: 992px) {
        div.line:nth-child(2){
            flex-direction: row;
        }
        div.line:nth-child(2) div:nth-child(2){
            margin-top: 0;
        }
        .reservation-col-paid p,
        .reservation-col-canceled p {
            text-align: left;
        }
        .reservation-col-buttons,
        .reservation-col-canceled,
        .screen-size-edit,
        .reservation-col-paid {
            max-width: 100%;
            flex: 0 0 100%;
        }
        .reservation-col-buttons{
            flex-direction: row;
            justify-content: space-between;
        }
        .reservation-col-buttons .button{
            width: 45%;
        }
        .email-form .button,
        .email-form .button .el-button 
        {
            flex-basis: unset;
        }
        .reservation{
            flex-direction: column;
        }
        .reservation-col-small {
            border-right: none;
            border-bottom: 1px solid var(--el-border-color);
            padding-bottom: 20px;
            max-width: 100%;
            flex: 0 0 100%;
        }
        .reservation-col-small div{
            margin-bottom: 20px;
            flex-basis: 37.5%;
        }
        .final-price{
            margin-top: 0;
        }
        .reservation-col-big {
            padding-left: unset;
            padding-top: 20px;
            max-width: 100%;
            flex: 0 0 100%;
        }
    }
    @media screen and (max-width: 680px) {
        div.line:nth-child(2) {
            flex-direction: column
        }
        div.line:nth-child(2) div:nth-child(2) {
            margin-top: 20px;
        }
    }
    @media screen and (max-width: 480px) {
        .email-form{
            flex-direction: column
        }
        .email-form .button,
        .email-form .button .el-button {
                flex-basis: 100%;
        }
        .email-form .el-form-item.is-required,
        .reservation-col-small div{
            flex-basis: 100%
        }
        div.line div{
            flex-basis: 50%;
        }
        div.line:nth-child(3) {
            flex-wrap: wrap;
        }
        div.line:nth-child(3) div:nth-child(3) {
            margin-top: 20px;
        }
    }
</style>
<style>
    .drawer-min-width {
        min-width: 320px;
    }
</style>