<template>
    <el-row v-if="rooms">
        <el-col :span="4">
            <filters :filterValues="filterValues" @filterRooms="filterRooms"/>
        </el-col>
        <el-col :span="20">
            <div class="rooms" v-if="rooms.length">
                <div v-for="room in rooms" :key="room.id" class="room">
                    <room :room="room" />
                    <el-checkbox :id="room.id.toString()" :value="room.id" class="room-checkbox" size="large" @change="updateSelectedRooms(room.id)" />
                </div>
                <el-button type="primary" :disabled="numberOfSelectedRooms ? false : true" size="large" class="reservation-button" @click="saveReservationRooms">
                    Rezervovat ({{ numberOfSelectedRooms }})
                </el-button>
            </div>
            <div v-else class="no-rooms">
                <p>Žádné pokoje k zobrazení!</p>
            </div>
        </el-col>
    </el-row>
    <el-row v-else class="no-rooms">
        <p>Žádné pokoje k zobrazení!</p>
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
                rooms: null,
                selectedRooms: [],
                filterValues: {
                    minPrice: 0,
                    maxPrice: 100,
                    minNumberOfBeds: 0,
                    maxNumberOfBeds: 10,
                    equipments: [{ value: 'TODO1', label: 'TODO1' }, { value: 'TODO2', label: 'TODO2'}]
                }
            };
        },
        created() {
            this.loadRooms();
        },
        computed: {
            numberOfSelectedRooms() {
                return this.selectedRooms.length
            }
        },
        methods: {
            loadRooms() {
                this.rooms = null;
                this.$root.loading = !this.$root.loading
                fetch('api/Room/GetAll')
                .then(r => r.json())
                .then(json => {
                    this.rooms = json;
                    this.filterValues.minNumberOfBeds = Math.min(...json.map(item => item.numberOfBeds));
                    this.filterValues.minPrice = Math.min(...json.map(item => item.costPerNight));
                    this.filterValues.maxNumberOfBeds = Math.max(...json.map(item => item.numberOfBeds));
                    this.filterValues.maxPrice = Math.max(...json.map(item => item.costPerNight));
                    this.$root.loading = !this.$root.loading
                    return;
                });
            },
            filterRooms(filterData) {
                let requestParams = this.$createRequestParams(filterData, true);
                console.log(requestParams)
                let requestBody = this.$createBodyParams(filterData);
                console.log(requestBody)
                this.$root.loading = !this.$root.loading
                fetch('api/Room/GetFiltered' + requestParams, {
                    method: 'POST',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(requestBody)
                })
                .then(r => r.json())
                .then(json => {
                    this.rooms = json;
                    console.log(json)
                    this.$root.loading = !this.$root.loading
                    return
                });
            },
            updateSelectedRooms(id) {
                if (!this.selectedRooms.includes(id)) {
                    this.selectedRooms.push(id);
                }
                else {
                    this.selectedRooms.splice(this.selectedRooms.indexOf(id), 1);
                }
            },
            saveReservationRooms() {
                this.$store.dispatch('saveReservationRooms', Object.values(this.selectedRooms))
                this.$router.push({path: '/dataily-rezervace'});
            }
        }
    };
</script>
<style scoped>
    .rooms{
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        justify-content: space-between;
        align-items: flex-start;
    }
    .room {
        border: 1px solid var(--el-border-color);
        flex-basis: 49%;
        padding: 20px;
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        transition: border var(--el-transition-duration);
        position: relative;
    }
    .room:hover{
        border: 1px solid var(--el-color-primary);
    }
    .room-checkbox{
        position: absolute;
        top: 15px;
        right: 20px;
    }
    .room-checkbox >>> .el-checkbox__inner {
       width: 25px!important;
       height: 25px!important;
    }

    .room-checkbox >>> .el-checkbox__inner::after{
        height: 13px;
        width: 7px;
        top: 2px;
        left: 8px;
    }
    .reservation-button{
        margin: 30px 0;
        flex-basis: 100%;
    }
    .no-rooms p{
        text-align: center;
        flex-basis: 100%;
    }
</style>