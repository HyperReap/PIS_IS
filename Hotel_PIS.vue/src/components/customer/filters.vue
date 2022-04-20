<template>
    <div class="filters">
        <h4>Filtry</h4>
        <el-date-picker v-model="filteredValues.dateFrom" type="date" placeholder="Datum příjezdu" popper-class="date-dropdown"
                        class="date-picker" :disabled-date="disabledDateFrom" format="DD. MM. YYYY" value-format="YYYY-MM-DD" />
        <el-date-picker v-model="filteredValues.dateTo" type="date" placeholder="Datum odjezdu" popper-class="date-dropdown"
                        class="date-picker" :disabled-date="disabledDateTo" format="DD. MM. YYYY" value-format="YYYY-MM-DD" />
        <p>Cena</p>
        <el-slider @change="setFilterPrice" range :min="filterValues.minPrice" :max="filterValues.maxPrice" class="range-picker" tooltip-class="range-picker-tooltip" />
        <p>Počet postelí</p>
        <el-slider @change="setFilterBeds" range :min="filterValues.minNumberOfBeds" :max="filterValues.maxNumberOfBeds" class="range-picker" tooltip-class="range-picker-tooltip" />
        <p>Vybavení:</p>
        <el-select v-model="filteredValues.equipments" multiple collapse-tags placeholder="Vybavení">
            <el-option v-for="item in equipments" :key="item.value" :label="item.label" :value="item.label" />
        </el-select>
        <el-button class="button" @click="filterRooms">Filtrovat</el-button>
    </div>
</template>
<script lang="js">
    export default {
        props: ['filterValues'],
        data() {
            return {
                filteredValues: {
                    equipments: [],
                    dateFrom: null,
                    dateTo: null,
                    minPrice: null,
                    maxPrice: null,
                    minNumberOfBeds: null,
                    maxNumberOfBeds: null                    
                },
                equipments: [],
                dayDuration: 86400000
            };
        },
        created() {
            this.loadAllEquipment();
        },
        methods: {
            disabledDateFrom(date) {
                return date.getTime() < Date.now() - this.dayDuration
            },
            disabledDateTo(date) {
                if (this.filteredValues.dateFrom === null) {
                    return date.getTime() < Date.now()
                }
                else {
                    return date.getTime() < this.getTimeStamp(this.filteredValues.dateFrom)
                }
            },
            getTimeStamp(strDate) {
                return ((new Date(strDate).getTime()))
            },
            setFilterPrice(prices) {
                this.filteredValues.minPrice = prices[0]
                this.filteredValues.maxPrice = prices[1]
            },
            setFilterBeds(beds) {
                this.filteredValues.minNumberOfBeds = beds[0]
                this.filteredValues.maxNumberOfBeds = beds[1]
            },
            filterRooms() {
                this.$emit('filterRooms', this.filteredValues)
            },
            loadAllEquipment() {
                fetch('api/Room/GetEquipments')
                .then(r => r.json())
                .then(json => {
                    let self = this;
                    Object.keys(json).forEach(function (key) {
                        self.equipments.push({ "label": json[key].name, "value": json[key].id });
                    });
                    return;
                });
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