<template>
    <newRoom @newRoom="newRoom" />
    <el-row v-if="rooms">
        <el-col :span="24" class="rooms">
            <el-card shadow="hover" v-for="room in rooms" class="room" :key="room.id" @click="openRoomDialog(room)">
                <h3>Pokoj {{room.roomNumber}}</h3>
                <p>Postele: {{room.numberOfBeds}}</p>
                <p>Přistýlky: {{room.numberOfSideBeds}}</p>
                <p>Cena: {{room.costPerNight}} Kč/noc</p>
            </el-card>
        </el-col>
    </el-row>
    <el-dialog v-model="dialogVisible" custom-class="new-room-dialog">
        <h3>Upravit pokoj</h3>
        <el-form :model="currentRoom" :rules="rules" ref="update-room" class="new-room-form">
            <el-form-item prop="roomNumber" label="Číslo pokoje">
                <el-input type="number" :min="1" placeholder="Číslo pokoje" v-model.number="currentRoom.roomNumber"></el-input>
            </el-form-item>
            <el-form-item prop="numberOfBeds" label="Počet postelí">
                <el-input type="number" :min="1" placeholder="Počet postelí" v-model.number="currentRoom.numberOfBeds"></el-input>
            </el-form-item>
            <el-form-item prop="numberOfSideBeds" label="Počet přistýlek">
                <el-input type="number" :min="0" placeholder="Počet přistýlek" v-model.number="currentRoom.numberOfSideBeds"></el-input>
            </el-form-item>
            <el-form-item prop="roomSize" label="Rozloha pokoje">
                <el-input type="number" :min="1" placeholder="Rozloha pokoje" v-model.number="currentRoom.roomSize"></el-input>
            </el-form-item>
            <el-form-item prop="floor" label="Patro">
                <el-input type="number" :min="0" placeholder="Patro" v-model.number="currentRoom.floor"></el-input>
            </el-form-item>
            <el-form-item prop="costPerNight" label="Cena za noc">
                <el-input type="number" :min="1" placeholder="Cena za noc" v-model.number="currentRoom.costPerNight"></el-input>
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button type="primary">Aktualizovat</el-button>
                <el-button type="danger" @click="deleteRoom(currentRoom.id)">Odstranit</el-button>
            </span>
        </template>
    </el-dialog>
</template>
<script lang="js">
    import newRoom from "@/components/admin/manager/newRoom.vue";
    import { ElMessage } from 'element-plus'
    export default {
        components: {
            newRoom,
            ElMessage
        },
        data() {
            return {
                rooms: [],
                currentRoom: {
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
                },
                dialogVisible: false
            };
        },
        created() {
            this.loadRooms();
        },
        methods: {
            loadRooms() {
                this.rooms = [];
                this.$root.loading = !this.$root.loading
                fetch('api/Room/GetAll', {
                    method: 'GET',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json',
                    },
                })
                .then(r => r.json())
                .then(json => {
                    this.rooms = json;
                    this.$root.loading = !this.$root.loading
                    ElMessage({ "message": "Pokoje načteny", "type": "success", "custom-class": "message-class" });
                })
                .catch(error => {
                    this.$root.loading = !this.$root.loading
                    ElMessage.error({ "message": "Nepodařilo se načíst pokoje!", "custom-class": "message-class", "grouping": true });
                    console.log(error);
                });
            },
            openRoomDialog(room) {
                this.currentRoom = room;
                this.dialogVisible = true;
            },
            newRoom(room) {
                this.rooms.push(room);
            },
            deleteRoom(id) {
                this.dialogVisible = false;
                fetch('api/Room/Delete?id=' + id, {
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
                        ElMessage({ "message": "Pokoj odebrán!", "type": "success", "custom-class": "message-class" });
                        this.rooms = this.rooms.filter(function (room) { return room.id !== id; });
                    }
                    else {
                        ElMessage.error({ "message": "Nepodařilo se odstranit pokoj!", "custom-class": "message-class", "grouping": true });
                    }
                    return;
                })
                .catch(error => {
                    ElMessage.error({ "message": "Nepodařilo se odstranit pokoj!", "custom-class": "message-class", "grouping": true });
                    console.log(error);
                });
            }
        }
    }
</script>
<style scoped>
    .rooms {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        justify-content: flex-start;
        margin-top: 20px;
    }

    .room {
        border: 1px solid red;
        width: 30%;
        flex-basis: 30%;
        margin-bottom: 20px;
        border: 1px solid var(--el-border-color);
        margin-right: 5%;
    }

    .room:hover {
        border: 1px solid var(--el-color-primary);
        cursor: pointer;
    }

    .room:nth-child(3n) {
        margin-right: 0;
    }

    .room p {
        text-overflow: ellipsis;
        overflow: hidden;
        width: 100%;
        white-space: nowrap;
    }

    h3 {
        font-weight: 500;
        margin-bottom: 20px;
    }

    .name {
        font-weight: 500;
        margin-bottom: 5px;
    }

    .room >>> .el-card__body {
        display: flex;
        flex-direction: column;
        flex-wrap: nowrap;
        justify-content: flex-start;
        align-items: flex-start;
    }

    .new-room-form >>> .date-picker,
    .new-room-form >>> .role-select {
        width: 100% !important;
    }

    .new-room-form >>> .date-picker input {
        padding-left: 30px !important;
    }

    .new-room-form {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        justify-content: space-between;
    }

    .new-room-form >>> .el-form-item {
        width: 47.5%;
        flex-basis: 47.5%;
        display: flex;
        flex-direction: column;
        align-items: flex-start;
    }
    .new-room-form >>> .el-form-item .el-form-item__content{
        width: 100%;
        flex-basis: 100%;
    }

    @media screen and (max-width: 992px) {
        .rooms {
            justify-content: space-between
        }

        .room {
            width: 45%;
            flex-basis: 45%;
            margin-right: 0;
        }

        .new-room-form >>> .el-form-item {
            width: 100%;
            flex-basis: 100%;
        }
    }

    @media screen and (max-width: 630px) {
        .room {
            width: 100%;
            flex-basis: 100%;
        }
    }
</style>