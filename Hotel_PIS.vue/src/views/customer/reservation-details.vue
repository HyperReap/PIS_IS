<template>
    <el-row>
        <el-col :span="24">
            <h2 :class="{ 'have-rooms': rooms.length }">Detaily rezervace</h2>
            <div v-if="rooms.length" :class="{ 'have-rooms': rooms.length }">
                <el-col :span="12" class="data">
                    <h3>Údaje</h3>
                    <el-form ref="reservation" :model="reservation" :rules="rules" class="data-form">
                        <el-form-item prop="dateFrom">
                            <el-date-picker v-model="reservation.dateFrom" type="date" placeholder="Datum příjezdu" popper-class="date-dropdown"
                                            class="date-picker" :disabled-date="disabledDateFrom" format="DD. MM. YYYY" value-format="YYYY-MM-DD" />
                        </el-form-item>
                        <el-form-item prop="dateTo">
                            <el-date-picker v-model="reservation.dateTo" type="date" placeholder="Datum odjezdu" popper-class="date-dropdown"
                                            class="date-picker" :disabled-date="disabledDateTo" format="DD. MM. YYYY" value-format="YYYY-MM-DD" />
                        </el-form-item>
                        <el-form-item prop="firstName">
                            <el-input type="text" placeholder="Jméno" v-model="reservation.firstName"></el-input>
                        </el-form-item>
                        <el-form-item prop="secondName">
                            <el-input type="text" placeholder="Příjmení" v-model="reservation.secondName"></el-input>
                        </el-form-item>
                        <el-form-item prop="email">
                            <el-input type="email" placeholder="Email" v-model="reservation.email"></el-input>
                        </el-form-item>
                        <el-form-item prop="phoneNumber">
                            <el-input type="tel" placeholder="Telefonní číslo" v-model="reservation.phoneNumber"></el-input>
                        </el-form-item>
                        <el-form-item v-for="room in rooms" :key="room.id" :prop="'numberOfPeople.room' + room.id">
                            <el-input type="number" :min="1" :max="room.numberOfBeds + room.numberOfSideBeds" :placeholder="'Počet osob v pokoji ' + room.roomNumber" v-model.number="reservation.numberOfPeople['room' + room.id]"></el-input>
                        </el-form-item>
                        <!--TODO: vypnout dokud nebude vyplneno vse -->
                        <el-form-item class="button">
                            <el-button type="primary" @click="createNewReservation">Rezervovat</el-button>
                        </el-form-item>
                    </el-form>
                    <ul>
                        <li>při vybírání data už musí být vypnuté obsazené termíny</li>
                        <li>odesilani na BE 2x (prvne klient, melo by se vratit jeho id ; pak rezervace, mel bych poslat id klienta)</li>
                        <li>mel bych si do store ulozit email klienta pro pouziti v mych rezervacich</li>
                        <li>mel bych presmerovat na stranku s detaily rezervace</li>
                    </ul>
                </el-col>
                <el-col :span="12" class="selected-rooms">
                    <h3>Zvolené pokoje</h3>
                    <div class="rooms">
                        <div v-for="room in rooms" :key="room.id" class="room">
                            <room :room="room" />
                        </div>
                    </div>
                </el-col>
            </div>
            <div v-else class="no-rooms">
                <h3>Nejsou vybrány žádné pokoje k rezervaci!</h3>
                <p class="countdown">Přesměrování na stránku pro výběr pokojů proběhne za {{countDown}} {{secWord}}</p>
            </div>
        </el-col>
    </el-row>
</template>
<script lang="js">
    import room from '@/components/customer/room.vue'
    import { ElMessage } from 'element-plus'
    export default {
        components: {
            room,
            ElMessage
        },
        data() {
            return {
                rooms: [1,2],
                countDownEnabled: false,
                countDown: 10,
                dayDuration: 86400000,
                reservation: {
                    dateFrom: null,
                    dateTo: null,
                    firstName: null,
                    secondName: null,
                    email: null,
                    phoneNumber: null,
                    numberOfPeople: {}
                },
            };
        },
        created() {
            //this.rooms = this.$store.getters.getReservationRooms.rooms;
            if (this.rooms.length > 0) {
                this.loadRooms()
            }
            else {
                this.countDownEnabled = true
            }
        },
        computed: {
            secWord() {
                let word;
                let secs = this.countDown;
                switch (true) {
                    case (secs == 0 || secs > 4):
                        word = "sekund"
                        break;
                    case (secs == 1):
                        word = "sekundu"
                        break;
                    case (secs >= 2 && secs <= 4):
                        word = "sekundy"
                        break;
                    default:
                        word = "sekund"
                }
                return word
            },
            rules() {
                let rules =
                {
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
                    phoneNumber: {
                        required: true,
                        message: "Zadejte platné telefonní číslo!",
                        pattern: "^[+]?[()0-9. -]{9,}$",
                        trigger: "blur",
                        autocomplete: "off"
                    },
                    dateFrom: {
                        type: "date",
                        required: true,
                        message: "Vyberte datum příjezdu!",
                        trigger: "blur",
                    },
                    dateTo: {
                        type: "date",
                        required: true,
                        message: "Vyberte datum odjezdu!",
                        trigger: "blur",
                    },
                    numberOfPeople: {}
                };
                return rules;
            }
        },
        methods: {
            loadRooms() {
                this.$root.loading = !this.$root.loading;
                let self = this;
                this.rooms.forEach(function (id, index) {
                    fetch('api/Room/Get?id=' + id, {
                        method: 'GET',
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json'
                        }
                    })
                    .then(r => r.json())
                    .then(room => {
                        self.rooms[index] = room;
                        self.reservation.numberOfPeople['room' + id] = null;
                        self.rules.numberOfPeople['room' + id] = [{ required: true, type: "number", message: "Zadejte správný počet osob!", pattern: "[0-9]+", trigger: "blur", min: 1, max: room.numberOfBeds + room.numberOfSideBeds, autocomplete: "off" }];
                        if (index === self.rooms.length - 1) {
                           self.$root.loading = !self.$root.loading;
                        }
                        return
                    })
                    .catch(error => {
                        self.$root.loading = false;
                        self.rooms = [];
                        self.countDownEnabled = true;
                        ElMessage.error({ "message": "Nepodařilo se načíst pokoje!", "custom-class": "message-class", "grouping": true });
                        console.log(error);
                    });
                });            
            },
            createNewReservation() {
                this.$refs.reservation.validate((result) => {
                    console.log(result)
                });
            },
            //TODO dodelat: chybi dam zohledneni dat, na ktere uz maji pokoje rezervaci
            disabledDateFrom(date) {
                return date.getTime() < Date.now() - this.dayDuration
            },
            disabledDateTo(date) {
                if (this.reservation.dateFrom === null) {
                    return date.getTime() < Date.now()
                }
                else {
                    return date.getTime() < this.getTimeStamp(this.reservation.dateFrom)
                }
            },
            getTimeStamp(strDate) {
                return ((new Date(strDate).getTime()))
            },
        },
        watch: {
            'reservation.dateFrom': function (newVal, oldVal) {
                if (newVal >= this.reservation.dateTo) {
                    this.reservation.dateTo = null
                }
            },
            countDownEnabled(value) {
                if (value) {
                    setTimeout(() => {
                        this.countDown--;
                    }, 1000);
                }
            },
            countDown: {
                handler(value) {
                    if (value > 0 && this.countDownEnabled) {
                        setTimeout(() => {
                            this.countDown--;
                        }, 1000);
                    }
                    if(value == 0 && this.countDownEnabled){
                        this.$router.push({ path: '/rezervace' });
                    }
                },
                immediate: true
            }
        }
    };
</script>
<style scoped>
    .selected-rooms{
        padding-left: 10px;
    }
    .data{
        padding-right: 10px;
    }
    h2 {
        margin-bottom: 20px;
        text-align: center;
    }
    h2.have-rooms{
        text-align: left;
        margin-bottom: 0;
    }
    .no-rooms h3,
    .no-rooms .countdown {
        text-align: center;
        margin-bottom: 10px;
    }
    .rooms {
        display: flex;
        flex-direction: column;
    }

    .room {
        border: 1px solid var(--el-color-primary);
        padding: 20px;
        display: flex;
        margin-bottom: 20px;
        flex-direction: row;
        flex-wrap: wrap;
        transition: border var(--el-transition-duration);
        position: relative;
    }
    .have-rooms h3{
        margin: 20px 0;
    }
    .data-form{
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        justify-content: space-between;
    }
    .data-form >>> .el-form-item.is-required {
        flex-basis: 47.5%
    }
    .data-form >>> .el-date-editor.el-input{
        width: 100%;
    }
    .data-form >>> .el-input-number{
        width: 100%;
    }
    .data-form >>> .el-input-number .el-input__inner{
        text-align: left;
    }
    .data-form >>> input::-webkit-outer-spin-button,
    .data-form >>> input::-webkit-inner-spin-button {
        -webkit-appearance: none;
        margin: 0;
    }
    .data-form >>> input[type=number] {
        -moz-appearance: textfield;
    }
    .button,
    .button >>> .el-button
    {
        flex-basis: 100%;
    }
</style>