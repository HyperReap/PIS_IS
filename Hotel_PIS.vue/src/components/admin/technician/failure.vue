<template>
    <el-container class="failure-wrap">
        <el-container class="failure" @click="dialogVisible = true">
            <p class="room-num">Pokoj {{ failure.roomNumber }}</p>
            <p class="desc">{{ failure.description }}</p>
            <p class="icon" v-if="failure.isSolved"><el-icon><check /></el-icon></p>
            <p class="icon" v-else><el-icon><close /></el-icon></p>
        </el-container>
    </el-container>
    <el-dialog v-model="dialogVisible" :title="'Závada na pokoji ' + failure.roomNumber">
        <p class="detail-solved">Vyřešeno: {{ this.yesOrNo(failure.isSolved) }}</p>
        <p class="detail-desc">{{ failure.description }}</p>
        <template #footer>
                <span class="dialog-footer">
                <el-button @click="dialogVisible = false">Zavřít</el-button>
                <el-button v-if="!failure.isSolved" type="primary" @click="solveFailure(failure.id)">Vyřešeno</el-button>
            </span>
        </template>
    </el-dialog>
</template>
<script setup>
import { ref } from 'vue';

const dialogVisible = ref(false)
</script>

<script>
export default {
    name: 'Failure',
    props: ['failure'],
    methods: {
        yesOrNo(isSolved) {
            if (isSolved) {
                return "Ano";
            }
            else {
                return "Ne";
            }
        },
        solveFailure(id) {
            this.dialogVisible = false;
            fetch('api/Failure/Solve?id=' + id, {
            method: 'GET'
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