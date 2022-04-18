<template>
    <div class="filters">
        <h4>Filtry</h4>
        <el-date-picker v-model="filteredValues.checkIn" type="date" placeholder="Datum příjezdu" popper-class="date-dropdown"
                        class="date-picker" :disabled-date="disabledDateCheckIn" format="DD. MM. YYYY" value-format="YYYY-MM-DD" />
        <el-date-picker v-model="filteredValues.checkOut" type="date" placeholder="Datum odjezdu" popper-class="date-dropdown"
                        class="date-picker" :disabled-date="disabledDateCheckOut" format="DD. MM. YYYY" value-format="YYYY-MM-DD" />
        <p>Cena</p>
        <el-slider @change="setFilterPrice" range :min="filterValues.minRoomPrice" :max="filterValues.maxRoomPrice" class="range-picker" tooltip-class="range-picker-tooltip" />
        <p>Počet postelí</p>
        <el-slider @change="setFilterBeds" range :min="filterValues.minNumberOfBeds" :max="filterValues.maxNumberOfBeds" class="range-picker" tooltip-class="range-picker-tooltip" />
        <p>Vybavení:</p>
        <el-select v-model="filteredValues.equipment" multiple collapse-tags placeholder="Vybavení">
            <el-option v-for="item in filterValues.equipment" :key="item.value" :label="item.label" :value="item.value" />
        </el-select>
        <el-button class="button">Filtrovat</el-button>
    </div>
</template>
<script lang="js">
    export default {
        props: ['filterValues'],
        data() {
            return {
                filteredValues: {
                    checkIn: null,
                    checkOut: null,
                    minRoomPrice: null,
                    maxRoomPrice: null,
                    minNumberOfBeds: null,
                    maxNumberOfBeds: null,
                    equipment: []
                },
                dayDuration: 86400000
            };
        },
        methods: {
            disabledDateCheckIn(date) {
                return date.getTime() < Date.now() - this.dayDuration
            },
            disabledDateCheckOut(date) {
                return date.getTime() < Date.now()
            },
            setFilterPrice(prices) {
                this.filteredValues.minRoomPrice = prices[0]
                this.filteredValues.maxRoomPrice = prices[1]
            },
            setFilterBeds(beds) {
                this.filteredValues.minNumberOfBeds = beds[0]
                this.filteredValues.maxNumberOfBeds = beds[1]
            }
        }
    }
</script>
<style scoped>
    .filters {
        background: var(--el-color-primary);
        color: white;
        position: fixed;
        width: 100%;
        padding: 20px;
        max-width: calc(16.6666666667% - 30px);
        display: flex;
        flex-direction: column;
    }
    h4 {
        margin-bottom: 20px;
        text-transform: uppercase;
    }
    .filters >>> .date-picker{
        width: 100%!important;
        margin-bottom: 20px;
    }
    .filters >>> .range-picker .el-slider__bar{
        background: #fff!important;
    }
    .filters >>> .range-picker .el-slider__runway{
        background: #a9abb0!important;
    }
    .filters >>> .range-picker .el-slider__button{
        border-color: #344055;
    }    
    .filters >>> .el-button.button{
        margin: 20px 0 0 auto;
    }
    .filters >>> .el-button.button:hover,
    .filters >>> .el-button.button:focus{
        color: var(--el-button-text-color);
        border-color: var(--el-button-border-color);
        background-color: var(--el-button-bg-color);
    }
</style>
<style>
    .date-dropdown *,
    .range-picker-tooltip,
    .el-select-dropdown__item {
        font-family: 'Roboto', Helvetica, Arial, sans-serif;
    }
</style>