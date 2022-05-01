<template>
    <el-button type="primary" @click="openDialog()">Přidat pokoj</el-button>
    <el-dialog v-model="dialogVisible" title="Přidat nový pokoj" custom-class="room-dialog">
        <el-form :model="room" :rules="rules" ref="room" class="new-room-form">
            <el-form-item prop="roomNumber">
                <el-input type="number" :min="1" placeholder="Číslo pokoje" v-model.number="room.roomNumber"></el-input>
            </el-form-item>
            <el-form-item prop="numberOfBeds">
                <el-input type="number" :min="1" placeholder="Počet postelí" v-model.number="room.numberOfBeds"></el-input>
            </el-form-item>
            <el-form-item prop="numberOfSideBeds">
                <el-input type="number" :min="0" placeholder="Počet přistýlek" v-model.number="room.numberOfSideBeds"></el-input>
            </el-form-item>
            <el-form-item prop="roomSize">
                <el-input type="number" :min="1" placeholder="Rozloha pokoje" v-model.number="room.roomSize"></el-input>
            </el-form-item>
            <el-form-item prop="floor">
                <el-input type="number" :min="0" placeholder="Patro" v-model.number="room.floor"></el-input>
            </el-form-item>
            <el-form-item prop="costPerNight">
                <el-input type="number" :min="1" placeholder="Cena za noc" v-model.number="room.costPerNight"></el-input>
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button type="primary" @click="add()">Přidat pokoj</el-button>
            </span>
        </template>
    </el-dialog>
</template>
<script lang="js">
    import { ElMessage } from 'element-plus'
    export default {
        name: 'newRoom',
        emits: ['newRoom'],
        components: {
            ElMessage,
        },
        data() {
            return {
                dialogVisible: false,
                room: {
                    numberOfBeds: null,
                    numberOfSideBeds: null,
                    roomSize: null,
                    floor: null,
                    roomNumber: null,
                    isCleaned: true,
                    costPerNight: null,
                    picture: ""
                },
                rules: {
                    roomNumber: {
                        required: true,
                        type: "number",
                        message: "Zadejte číslo pokoje!",
                        pattern: "[0-9]+",
                        trigger: "blur",
                        min: 1,
                        autocomplete: "off"
                    },
                    numberOfBeds: {
                        required: true,
                        type: "number",
                        message: "Zadejte počet postelí!",
                        pattern: "[0-9]+",
                        trigger: "blur",
                        min: 1,
                        autocomplete: "off"
                    },
                    numberOfSideBeds: {
                        required: true,
                        type: "number",
                        message: "Zadejte počet přistýlek!",
                        pattern: "[0-9]+",
                        trigger: "blur",
                        min: 0,
                        autocomplete: "off"
                    },
                    roomSize: {
                        required: true,
                        type: "number",
                        message: "Zadejte rozlohu pokoje!",
                        pattern: "[0-9]+",
                        trigger: "blur",
                        min: 1,
                        autocomplete: "off"
                    },
                    floor: {
                        required: true,
                        type: "number",
                        message: "Zadejte patro!",
                        pattern: "[0-9]+",
                        trigger: "blur",
                        min: 0,
                        autocomplete: "off"
                    },
                    costPerNight: {
                        required: true,
                        type: "number",
                        message: "Zadejte cenu za noc!",
                        pattern: "[0-9]+",
                        trigger: "blur",
                        min: 1,
                        autocomplete: "off"
                    },
                }
            };
        }, 
        methods: {
            openDialog() {
                this.dialogVisible = !this.dialogVisible;
            },
            add() {
                this.$refs.room.validate((result) => {
                    console.log(result)
                    if (result) {
                        fetch('api/Room/Save', {
                            method: 'POST',
                            headers: {
                                'Accept': 'application/json',
                                'Content-Type': 'application/json',
                                'Authorization': 'Bearer ' + this.$store.getters.getLoggedUserToken
                            },
                            body: JSON.stringify(this.room)
                        })
                        .then(r => r.json())
                        .then(json => {
                            this.dialogVisible = !this.dialogVisible;
                            ElMessage({ "message": "Pokoj přidán", "type": "success", "custom-class": "message-class", "grouping": true });
                            if ("id" in json && "roomNumber" in json && "numberOfBeds" in json) {
                                this.$emit('newRoom', json);
                                this.$refs.room.resetFields();
                            }
                            else {
                                ElMessage({ "message": "Aktualizujte stránku pro nová data", "type": "success", "custom-class": "message-class", "grouping": true });
                            }
                            return
                        })
                        .catch(error => {
                            this.dialogVisible = !this.dialogVisible;
                            ElMessage.error({ "message": "Nepodařilo se přidat pokoj!", "custom-class": "message-class", "grouping": true });
                            console.log(error);
                        });
                    }
                });
            }
        }
    }
</script>
<style scoped>
    .new-room-form >>> .date-picker,
    .new-room-form >>> .role-select
    {
        width: 100% !important;
    }
    .new-room-form >>> .date-picker input,
    .new-room-form >>> .password input
    {
        padding-left: 30px!important;
    }
    .new-room-form {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        justify-content: space-between;
    }
    .new-room-form >>> .el-form-item{
        width: 47.5%;
        flex-basis: 47.5%;
    }
    .new-room-form >>> .el-input-number .el-input__inner{
        text-align: left;
    }
    .new-room-form >>> input::-webkit-outer-spin-button,
    .new-room-form >>> input::-webkit-inner-spin-button {
        -webkit-appearance: none;
        margin: 0;
    }
    .new-room-form >>> input[type=number] {
        -moz-appearance: textfield;
    }
    @media screen and (max-width: 992px) {
        .new-room-form >>> .el-form-item {
            width: 100%;
            flex-basis: 100%;
        }
    }
</style>