<template>
    <newFailure @newFailure="newFailure" />
    <el-row v-if="failures">
        <el-col :span="24" class="failures">
            <div :class="[{ 'disable-pointer-events': !isTechnician }, 'failure']" v-for="failure in failures" :key="failure.id" @click="openFailureDialog(failure)">
                <p><strong>Pokoj {{ failure.roomNumber }}</strong></p>
                <p>{{ failure.description }}</p>
            </div>
        </el-col>
    </el-row>
    <el-dialog v-model="dialogVisible" custom-class="new-failure-dialog">
        <h3>Pokoj {{currentlySolvedFailure.roomNumber}}</h3>
        <p><strong>Popis závady:</strong></p>
        <p class="detail-desc">{{ currentlySolvedFailure.description }}</p>
        <template #footer>
            <span class="dialog-footer">
                <el-button type="primary" @click="solveFailure(currentlySolvedFailure.id)">Označit jako vyřešeno</el-button>
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
                dialogVisible: false,
                isTechnician: false
            };
        },
        created() {
            this.loadFailures();
            this.isTechnician = (this.$store.getters.getLoggedUserRole === 3) ? true : false;
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
                    ElMessage({ "message": "Závady načteny", "type": "success", "custom-class": "message-class" });
                })
                .catch(error => {
                    this.$root.loading = !this.$root.loading
                    ElMessage.error({ "message": "Nepodařilo se načíst závady!", "custom-class": "message-class", "grouping": true });
                    console.log(error);
                });
            },
            openFailureDialog(failure) {
                if (this.isTechnician) {
                    this.currentlySolvedFailure = failure;
                    this.dialogVisible = true;
                }                
            },
            solveFailure(id) {
                this.dialogVisible = false;
                fetch('api/Failure/Solve?failureId=' + id, {
                    method: 'GET',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                })
                .then(r => {
                    if (r.status === 200) {
                        ElMessage({ "message": "Vyřešeno!", "type": "success", "custom-class": "message-class" });
                        this.failures = this.failures.filter(function (failure) { return failure.id !== id; });
                    }
                    else {
                        ElMessage.error({ "message": "Nepodařilo se označit jako vyřešeno!", "custom-class": "message-class", "grouping": true });
                    }
                    return;
                })
                .catch(error => {
                    ElMessage.error({ "message": "Nepodařilo se označit jako vyřešeno!", "custom-class": "message-class", "grouping": true });
                    console.log(error);
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
        justify-content: flex-start;
        margin-top: 20px;
    }
    .failure {
        border: 1px solid red;
        width: 30%;
        flex-basis: 30%;
        margin-bottom: 20px;
        padding: 20px;
        border: 1px solid var(--el-border-color);
        margin-right: 5%;
    }
    .failure:nth-child(3n){
        margin-right: 0;
    }
    .failure p {
        text-overflow: ellipsis;
        overflow: hidden;
        width: 100%;
        white-space: nowrap;
     }
    .failure:hover{
        border: 1px solid var(--el-color-primary);
        cursor: pointer;
    }
    .failure.disable-pointer-events{
        pointer-events: none;
    }
    strong{
        font-weight: 500;
        display: block;
    }
    .failure strong{
        margin-bottom: 20px;
    }
    h3{
        font-weight: 500;
        margin-bottom: 20px;
        font-size: 18px;
    }
    @media screen and (max-width: 992px) {
        .failures{
            justify-content: space-between
        }
        .failure {
            width: 45%;
            flex-basis: 45%;
            margin-right: 0;
        }
    }
    @media screen and (max-width: 630px) {
        .failure {
            width: 100%;
            flex-basis: 100%;
        }
    }
</style>