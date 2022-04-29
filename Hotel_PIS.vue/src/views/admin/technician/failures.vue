<template>
    <newFailure @newFailure="newFailure"/>
    <el-row v-if="failures">
        <el-col :span="24">
            <failure v-for="failure in failures" :key="failure.id" :failure="failure" />
        </el-col>
    </el-row>
</template>
<script lang="js">
    import failure from "@/components/admin/technician/failure.vue";
    import newFailure from "@/components/admin/technician/newFailure.vue";
    import { ElMessage } from 'element-plus'
    export default {
        components: {
            failure,
            newFailure,
            ElMessage
        },
        data() {
            return {
                failures: [],
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
            newFailure(failure) {
                this.failures.push(failure);
            }
        }
    }
</script>
<style scoped>
    .failures {
        width: 100%;
        height: 100%;
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
    }
</style>