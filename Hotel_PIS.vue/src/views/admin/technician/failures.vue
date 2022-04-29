<template>
    <newFailure @newFailure="newFailure" />
    <el-row v-if="failures">
        <el-col :span="24" class="failures">
            <div class="failure" v-for="failure in failures" :key="failure.id" @click="openFailureDialog(failure)">
                <p>Pokoj {{ failure.roomNumber }}</p>
                <p>{{ failure.description }}</p>
            </div>
        </el-col>
    </el-row>
    <el-dialog v-model="dialogVisible" :title="'Závada na pokoji ' + currentlySolvedFailure.roomNumber">
        <p><strong>Popis závady:</strong></p>
        <p class="detail-desc">{{ currentlySolvedFailure.description }}</p>
        <template #footer>
                <span class="dialog-footer">
                <el-button type="primary" @click="solveFailure(failure.id)">Označit jako vyřešeno</el-button>
            </span>
        </template>
    </el-dialog>
</template>
<script lang="js">
    import newFailure from "@/components/admin/technician/newFailure.vue";
    import { ElMessage } from 'element-plus'
    export default {
        components: {
            newFailure,
            ElMessage
        },
        data() {
            return {
                failures: [],
                currentlySolvedFailure: {
                    id: null,
                    roomId: null,
                    roomNumber: null,
                    description: null,
                    isSolved: null
                },
                dialogVisible: false
            };
        },
        created() {
            this.loadFailures();
        },
        methods: {
            loadFailures() {
                this.failures = [];
                this.$root.loading = !this.$root.loading
                fetch('api/Failure/GetAll')
                .then(r => r.json())
                .then(json => {
                    this.failures = json;
                    this.$root.loading = !this.$root.loading
                    ElMessage({ "message": "Závada načteny", "type": "success", "custom-class": "message-class" });
                })
                .catch(error => {
                    this.$root.loading = !this.$root.loading
                    ElMessage.error({ "message": "Nepodařilo se načíst závady!", "custom-class": "message-class", "grouping": true });
                    console.log(error);
                });
            },
            openFailureDialog(failure) {
                console.log(failure)
            },
            solveFailure(id) {
                this.dialogVisible = false;
                fetch('api/Failure/Solve?id=' + id, {
                    method: 'GET'
                });
            },
            newFailure(failure) {
                this.failures.push(failure);
            }
        }
    }
</script>
<style scoped>
    .failures {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        justify-content: space-between;
        margin-top: 20px;
    }
    .failure {
        border: 1px solid red;
        width: 30%;
        flex-basis: 30%;
        margin-bottom: 20px;
        padding: 20px;
        border: 1px solid var(--el-border-color);
    }
    .failure:hover{
        border: 1px solid var(--el-color-primary);
        cursor: pointer;
    }
</style>