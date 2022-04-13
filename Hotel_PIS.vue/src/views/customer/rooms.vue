<template>
    <el-row>
        <el-col :span="4">
            <filters />
        </el-col>
        <el-col :span="20">
            <div v-if="loading" class="loading">
                Loading... TODO
            </div>
            <div v-if="rooms">
                <div v-for="room in rooms" :key="room.id" class="room">
                    <room :room="room" />
                </div>
            </div>
        </el-col>
    </el-row>
</template>
<script lang="js">
    import filters from '@/components/customer/filters.vue'
    import room from '@/components/customer/room.vue'
    export default {
        components: {
            filters,
            room
        },
        data() {
            return {
                loading: false,
                rooms: null
            };
        },
        created() {
            this.fetchData();
        },
        methods: {
            fetchData() {
                this.rooms = null;
                this.loading = true;

                fetch('api/Room/GetAll')
                    .then(r => r.json())
                    .then(json => {
                        this.rooms = json;
                        this.loading = false;
                        return;
                    });
            }
        }
    };
</script>
<style scoped>
    .room{
        border: 1px solid red;
    }
</style>