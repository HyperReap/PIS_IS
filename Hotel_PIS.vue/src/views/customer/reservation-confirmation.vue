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
        <el-col :span="4" class="reservation-col-buttons">
            <el-button type="primary" class="button">Zaplatit zálohu</el-button>
            <el-popconfirm confirmButtonType="danger" confirm-button-text="Ano" cancel-button-text="Ne" icon-color="red" title="Opravdu chcete zručit rezervaci?" @confirm="cancelReservation(reservation.reservationId)">
                <template #reference>
                    <el-button type="danger" class="button">Zrušit rezervaci</el-button>
                </template>
            </el-popconfirm>            
        </el-col>
    </el-row>
</template>
<script lang="js">
    import { ElMessage } from 'element-plus'
    export default {
        components: {
            ElMessage
        },
        data() {
            return {
                reservations: [
                    {
                        "roomId": 1,
                        "numberOfPeople": 3,
                        "dateTo": "2024-05-09",
                        "dateFrom": "2024-05-02",
                        "firstName": "Ondřej",
                        "secondName": "Studnička",
                        "email": "studnicka39@seznam.cz",
                        "phoneNumber": "732564138",
                        "cost": 847,
                        "reservationId": 19,
                        "roomNumber": 11111
                    },
                    {
                        "roomId": 2,
                        "numberOfPeople": 150,
                        "dateTo": "2024-05-05",
                        "dateFrom": "2024-05-02",
                        "firstName": "Ondřej",
                        "secondName": "Studnička",
                        "email": "studnicka39@seznam.cz",
                        "phoneNumber": "732564138",
                        "cost": 1484,
                        "reservationId": 20,
                        "roomNumber": 22222
                    }
                ],
                days: ['Ne', 'Po', 'Út', 'St', 'Čt', 'Pá', 'So']
            };
        },
        //TODO: created - nacist data ze store a nasledne je okamzite vymazat, pokud bude store prazdny tak redirect na nova rezervace
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
            cancelReservation(id) {
                console.log(id)
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

</style>