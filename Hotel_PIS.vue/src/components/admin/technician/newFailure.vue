<template>
    <el-button type="primary" @click="openDialog()">Přidat závadu</el-button>
    <el-dialog v-model="dialogVisible" title="Přidat novou závadu">
        <el-form :model="failure" :rules="rules" ref="failure">
            <el-form-item prop="roomId">
                <el-select v-model="failure.roomId" placeholder="Vybrat pokoj">
                    <el-option v-for="room in rooms" :key="room.id" :label="'Pokoj ' + room.roomNumber" :value="room.id" />
                </el-select>
            </el-form-item>
            <el-form-item prop="description">
                <el-input type="text" v-model="failure.description" placeholder="Popis závady" />
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button type="primary" @click="sendFailure()">Odeslat</el-button>
            </span>
        </template>
    </el-dialog>
</template>
<script lang="js">
    import { ElMessage } from 'element-plus'
    export default {
        name: 'NewFailure',
        emits: ['newFailure'],
        components: {
            ElMessage,
        },
        data() {
            return {
                rooms: [],
                dialogVisible: false,
                failure: {
                    roomId: null,
                    description: null
                },
                rules: {
                    roomId: {
                        required: true,
                        message: "Vyberte místnost!",
                        trigger: "change",
                    },
                    description: {
                        required: true,
                        message: "Zadejte popis závady!",
                        trigger: "blur",
                        min: 5,
                        max: 255,
                        autocomplete: "off"
                    },
                }
            };
        }, 
        methods: {
            openDialog() {
                this.dialogVisible = !this.dialogVisible;
                this.loadRoomNumbers();
            },
            loadRoomNumbers() {
                fetch('api/Room/GetAll')
                .then(r => r.json())
                .then(json  => {
                    this.rooms = json;
                })
                .catch(error => {
                    this.dialogVisible = !this.dialogVisible;
                    ElMessage.error({ "message": "Nepodařilo se načíst pokoje!", "custom-class": "message-class" });
                    console.log(error);
                });
            },    
            sendFailure() {
                this.$refs.failure.validate((result) => {
                    if (result) {
                        fetch('api/Failure/Save', {
                            method: 'POST',
                            headers: {
                                'Accept': 'application/json',
                                'Content-Type': 'application/json'
                            },
                            body: JSON.stringify(this.failure)
                        })
                        .then(r => r.json())
                        .then(json => {
                            this.dialogVisible = !this.dialogVisible;
                            ElMessage({ "message": "Závada nahlášena", "type": "success", "custom-class": "message-class", "grouping": true });
                            if ("id" in json && "roomId" in json && "description" in json) {
                                this.$emit('newFailure', json);
                            }
                            else {
                                ElMessage({ "message": "Aktualizujte stránku pro nová data", "type": "success", "custom-class": "message-class", "grouping": true });
                            }
                            return
                        })
                        .catch(error => {
                            this.dialogVisible = !this.dialogVisible;
                            ElMessage.error({ "message": "Nepodařilo se nahlásit závadu!", "custom-class": "message-class", "grouping": true });
                            console.log(error);
                        });
                    }
                });
            }
        }
    }
</script>
<style scoped>
.failure-wrap {
  height: 70px;
  margin: 2%;
  flex-basis: 21%;
  flex-grow: 0;
  border: 1px solid var(--el-border-color);
}

.failure-wrap:hover{
  border: 1px solid var(--el-color-primary);
}

.failure {
  text-align: center;
}

.room-num {
  width: 25%;
  padding: 5%;
  border-right: 3px dashed var(--el-border-color);
  background-color: var(--el-fill-color-lighter);
}

.desc {
  width: 65%;
  padding: 3%;
  text-align: left;
  text-overflow: ellipsis;
  overflow: hidden;
  white-space: nowrap;
}

.icon {
  width: 5%;
  display: flex;
  align-items: center;
  border-left: 3px dashed var(--el-border-color);
}

.el-icon {
  margin: 3px;
}

.detail-solved {
  padding-bottom: 4px;
  border-bottom: 1px solid var(--el-border-color);
}

.detail-desc {
  padding-top: 4px;
}
</style>